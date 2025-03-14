using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;
using System.Security.Cryptography;
using System.Text;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            user.PasswordHash = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user.PasswordHash)));
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (foundUser == null || foundUser.PasswordHash != Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user.PasswordHash))))
            {
                return Unauthorized();
            }

            var token = _jwtService.GenerateToken(foundUser.Username);
            return Ok(new { token });
        }
    }
}
