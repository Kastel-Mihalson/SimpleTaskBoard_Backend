using Auth.API.Models;
using Auth.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleTaskBoard.Domain.Models;
using SimpleTaskBoard.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly IRepositoryWrapper _repository;

        public AuthController(IOptions<AuthOptions> authOptions, IRepositoryWrapper repository)
        {
            _authOptions = authOptions;
            _repository = repository;
        }

        public IReadOnlyList<User> Users => new List<User>
        {
            new User
            {
                Id = Guid.Parse("2845A861-8679-469C-B97D-E931E1638BBF"),
                Name = "user",
                Email = "user@email.ru",
                Password = "user"
            },
            new User
            {
                Id = Guid.Parse("070839D5-4774-44F5-843C-10370F575798"),
                Name = "admin",
                Email = "admin@email.ru",
                Password = "admin"
            },
            new User
            {
                Id = Guid.Parse("4E0B3ECE-A245-40E3-A219-79FB54EDD541"),
                Name = "test",
                Email = "test@email.ru",
                Password = "test"
            }
        };

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = await AuthentificateUser(login.Email, login.Password);

            if (user != null)
            {
                var token = GenerateJWT(user);
                return Ok(new
                {
                    access_token = token
                });
            }
            return Unauthorized();
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repository.User.GetAllUsers();

            return Ok(users);
        }

        [HttpGet("get-user-by-email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _repository.User.GetUserByEmail(email);

            if (user == null)
            {
                return NotFound($"User by email: {email} not found");
            }

            return Ok(user);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var userId = Guid.NewGuid();

            _repository.User.Create(new User
            {
                Id = userId,
                Email = user.Email,
                Name = user.Name,
                Password = user.Password
            });
            await _repository.SaveAsync();

            return Ok($"User succesfully created. Id: {userId}");
        }

        private async Task<User?> AuthentificateUser(string email, string password)
        {
            var user = await _repository.User.GetUserByEmail(email);

            return user != null && user.Password.Equals(password)
                ? user
                : null;
        }

        private string GenerateJWT(User user)
        {
            var authParams = _authOptions.Value;
            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromSeconds(authParams.TokenLifetime)),
                Issuer = authParams.Issuer,
                Audience = authParams.Audience,
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
