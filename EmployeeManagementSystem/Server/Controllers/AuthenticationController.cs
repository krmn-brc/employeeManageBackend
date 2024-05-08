using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserAccount _userAccount;

        public AuthenticationController(IUserAccount userAccount)
        {
            _userAccount = userAccount;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(Register user)
        {
            if(user is null)
            {
                return BadRequest("Model is empty");
            }
            var result  = await _userAccount.CreateAsync(user);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(Login user)
        {
            if(user == null) return BadRequest("Model is empty!");
            var result = await _userAccount.SignInAsync(user);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshToken token)
        {
            if(token == null) return BadRequest("Model is empty!");
            var result = await _userAccount.RefreshTokenAsync(token);
            return Ok(result);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _userAccount.GetUsersAsync();
            if(users == null) return NotFound();
            return Ok(users);
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUserAsync(ManageUser user)
        {
            var result = await _userAccount.UpdateUserAsync(user);
            return Ok(result);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRolesAsync()
        {
            var roles = await _userAccount.GetSystemRolesAsync();
            if(roles == null) return NotFound();
            return Ok(roles);
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var result = await _userAccount.DeleteUserAsync(id);
            return Ok(result);
        }
    }
}