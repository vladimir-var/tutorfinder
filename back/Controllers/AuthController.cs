using Microsoft.AspNetCore.Mvc;
using tutorfinder.DTOs;
using tutorfinder.Services;
using BCrypt.Net;

namespace tutorfinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Невірний email або пароль" });
            }

            var userWithPassword = await _userService.GetUserWithPasswordAsync(loginDto.Email);
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, userWithPassword.PasswordHash))
            {
                return Unauthorized(new { message = "Невірний email або пароль" });
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new { 
                token = token,
                userType = user.Role
            });
        }
    }
} 