using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class UserAccountRepository : IUserAccount
    {
        private readonly IOptions<JwtSection> _jwtSection;
        private readonly AppDbContext _appDbContext;

        public UserAccountRepository(IOptions<JwtSection> jwtSection, AppDbContext appDbContext)
        {
            _jwtSection = jwtSection;
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            if(user == null )
              return  new GeneralResponse(false, "Model is empty!");
            var checkUser = await FindUserByEmailAsync(user.Email!);
            if(checkUser != null)
                return new GeneralResponse(false, "User registered already");
            ApplicationUser applicationUser =  await AddToDatabase(new ApplicationUser
            {
                FullName = user.FullName,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            });
            var checkAdminRole = await _appDbContext.SystemRoles.FirstOrDefaultAsync(x => x.Name!.Equals(Constants.Admin));
            if(checkAdminRole == null)
            {
                var createAdminRole = await AddToDatabase(new SystemRole{Name = Constants.Admin});
                await AddToDatabase(new UserRole{RoleId = createAdminRole.Id, UserId = applicationUser.Id});
                return new GeneralResponse(true, "Account created!");
            }
            var checkUserRole = await _appDbContext.SystemRoles.FirstOrDefaultAsync(x => x.Name!.Equals(Constants.User));
            SystemRole response = new();
            if(checkUserRole == null)
            {
                response = await AddToDatabase(new SystemRole{Name = Constants.User});
                await AddToDatabase(new UserRole{RoleId = response.Id, UserId= applicationUser.Id});
            }
            else
                await AddToDatabase(new UserRole{RoleId = checkUserRole.Id, UserId= applicationUser.Id});
            return new GeneralResponse(true, "Account created!");
        }


        public async Task<LoginResponse> SignInAsync(Login user)
        {
            if(user is null)
                return new LoginResponse(false,  "Model is empty!");
            
            var applicationUser = await FindUserByEmailAsync(user.Email);
            if(applicationUser is null) 
                return new LoginResponse(false, "User not found");

            if(!BCrypt.Net.BCrypt.Verify(user.Password, applicationUser.Password))
                return new LoginResponse(false, "Email or Password not valid.");
            
            UserRole? getUserRole = await FindUserRoleAsync(applicationUser.Id);
            if(getUserRole == null)
                 return new LoginResponse(false, "User role not found");

            SystemRole? getRoleName = await FindRoleNameAsync(getUserRole.RoleId);
            
            string jwtToken = GenerateToken(applicationUser, getRoleName!.Name);
            string jwtRefreshToken = GenerateRefreshToken();
            
            var findUser = await _appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(x => x.UserId == applicationUser.Id);
            if (findUser is not null)
            {
                findUser!.Token = jwtRefreshToken;
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                await AddToDatabase(new RefreshTokenInfo(){Token = jwtRefreshToken, UserId = applicationUser.Id});
            }

            return new LoginResponse(true, "Login successfully", jwtToken, jwtRefreshToken);
        }

        private string GenerateToken(ApplicationUser user, string? role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSection.Value.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role!)
            };

            var token = new JwtSecurityToken
            (
                issuer: _jwtSection.Value.Issuer,
                audience: _jwtSection.Value.Audience,
                claims: userClaims,
                expires:DateTime.Now.AddSeconds(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        
        private async Task<UserRole?> FindUserRoleAsync(int userId) => await _appDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);
        private async Task<SystemRole?> FindRoleNameAsync(int roleId) => await _appDbContext.SystemRoles.FirstOrDefaultAsync(x => x.Id == roleId);
        private async Task<ApplicationUser?> FindUserByEmailAsync(string? email)
         =>  await _appDbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email!.ToLower().Equals(email!.ToLower()));
       
        private async Task<T> AddToDatabase<T>(T model)
        {
            var result =  _appDbContext.Add(model!);
            await _appDbContext.SaveChangesAsync();
            return (T)result.Entity;
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            if(token == null) return new LoginResponse(false, "Model is empty!");

            var findToken = await _appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(x => x.Token!.Equals(token.Token));
            if(findToken == null) return new LoginResponse(false, "Refresh token is required!");

            var user = await _appDbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == findToken.UserId);
            if(user == null) return new LoginResponse(false, "Refresh token could not be generated because user not found!");

            var userRole = await FindUserRoleAsync(user.Id);
            var roleName = await FindRoleNameAsync(userRole!.RoleId);
            string jwtToken = GenerateToken(user, roleName!.Name);
            string refreshToken = GenerateRefreshToken();

            var updateRefreshToken = await _appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if(updateRefreshToken == null) return new LoginResponse(false,"Refresh token could not be generated because user has not signed in");

            updateRefreshToken.Token = refreshToken;
            await _appDbContext.SaveChangesAsync();
            return new LoginResponse(true, "Token refreshed successfully", jwtToken, refreshToken);
        }

        public async Task<ICollection<ManageUser>> GetUsersAsync()
        {
            var allUsers = await ApplicationUsersAsync();
            var allUserRoles = await UserRolesAsync();
            var allRoles = await SystemRolesAsync();
            if(allUsers.Count == 0 || allRoles.Count == 0) return null!;

            var users = new List<ManageUser>();
            foreach(var user in allUsers)
            {
                var userRole = allUserRoles.FirstOrDefault(x => x.UserId == user.Id);
                var roleName = allRoles.FirstOrDefault(x => x.Id == userRole!.RoleId);
                users.Add(new ManageUser{UserId = user.Id, Name = user.FullName!, Email = user.Email!, Role = roleName!.Name!});
            }
            return users;
        }

        public async Task<GeneralResponse> UpdateUserAsync(ManageUser user)
        {
            var getRole = (await SystemRolesAsync()).FirstOrDefault(x => x.Name!.Equals(user.Role));
            var userRole = await _appDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.UserId);
            userRole!.RoleId = getRole!.Id;
            await _appDbContext.SaveChangesAsync();
            return new GeneralResponse(true, "User role updated successfully");
        }

        public async Task<ICollection<SystemRole>> GetSystemRolesAsync()
        => await SystemRolesAsync();

        public async Task<GeneralResponse> DeleteUserAsync(int id)
        {
            var user = await _appDbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == id);
            _appDbContext.ApplicationUsers.Remove(user!);
            await _appDbContext.SaveChangesAsync();
            return new GeneralResponse(true, "User successfully deleted");
        }

        private async Task<List<ApplicationUser>> ApplicationUsersAsync()
        => await _appDbContext.ApplicationUsers.AsNoTracking().ToListAsync();

        private async Task<List<UserRole>> UserRolesAsync()
        => await _appDbContext.UserRoles.AsNoTracking().ToListAsync();

        private async Task<List<SystemRole>> SystemRolesAsync()
        => await _appDbContext.SystemRoles.AsNoTracking().ToListAsync();
    }
}