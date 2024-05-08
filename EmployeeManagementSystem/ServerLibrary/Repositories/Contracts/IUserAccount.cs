using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAsync(Register register);
        Task<LoginResponse> SignInAsync(Login login);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
         Task<ICollection<ManageUser>> GetUsersAsync();
        Task<GeneralResponse> UpdateUserAsync(ManageUser user);
        Task<ICollection<SystemRole>> GetSystemRolesAsync();
        Task<GeneralResponse> DeleteUserAsync(int id);
    }
}