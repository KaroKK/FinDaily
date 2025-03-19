using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FinBackend.Api.Models;
using FinBackend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FinBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly FinContext _db;
        private readonly SecurityKey _securityKey;

        public UsersController(FinContext db, SecurityKey securityKey)
        {
            _db = db;
            _securityKey = securityKey;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("User already exists");

            var newUser = new User
            {
                Username = dto.Username,
                PassHash = HashPwd(dto.Password)
            };
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            return Ok(new { userId = newUser.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null) return BadRequest("User not found");

            var hashed = HashPwd(dto.Password);
            if (user.PassHash != hashed) return BadRequest("Bad password");

            var token = MakeToken(user);
            return Ok(new { token });
        }

        private string HashPwd(string raw)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return Convert.ToBase64String(bytes);
        }

        private string MakeToken(User user)
        {
            var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

            var desc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userId", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(desc);
            return handler.WriteToken(token);
        }
    }

    public class RegDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}