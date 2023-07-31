using Auth.API.Models;
using Auth.API.Repositories;
using Auth.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly IRepository<User> _userRepository;

        public AuthController(IOptions<AuthOptions> authOptions, IRepository<User> userRepository)
        {
            _authOptions = authOptions;
            _userRepository = userRepository;
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

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {
            var user = AuthentificateUser(login.Email, login.Password);
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

        [Route("get-user-by-email/{id}")]
        [HttpGet]
        public IActionResult GetUserByEmail(Guid id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
            {
                return NotFound($"User by id: {id} not found");
            }

            return Ok(user);
        }

        [Route("create-user")]
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            var userId = Guid.NewGuid();

            _userRepository.Create(new User
            {
                Id = userId,
                Email = user.Email,
                Name = user.Name,
                Password = user.Password
            });

            return Ok($"User succesfully created. Id: {userId}");
        }

        private User? AuthentificateUser(string email, string password)
        {
            return Users.FirstOrDefault(u => u.Email == email && u.Password == password);
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
