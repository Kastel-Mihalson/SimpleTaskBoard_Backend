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
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IOptions<AuthOptions> authOptions, IUnitOfWork unitOfWork)
        {
            _authOptions = authOptions;
            _unitOfWork = unitOfWork;
        }

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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserData userData)
        {
            var user = await AuthentificateUser(userData.Email, userData.Password);

            if (user != null)
            {
                return BadRequest($"User with email: {user.Email} already exist");
            }

            if (user == null)
            {
                await CreateUser(userData);
            }

            return Ok("User succesfully registered.");
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.User.GetAllUsers();

            return Ok(users);
        }

        [HttpGet("get-user-by-email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _unitOfWork.User.GetUserByEmail(email);

            if (user == null)
            {
                return NotFound($"User by email: {email} not found");
            }

            return Ok(user);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] UserData user)
        {
            var userId = Guid.NewGuid();

            _unitOfWork.User.Create(new User
            {
                Id = userId,
                Email = user.Email,
                Name = user.Name,
                Password = user.Password
            });
            await _unitOfWork.SaveAsync();

            return Ok($"User succesfully created. Id: {userId}");
        }

        private async Task<User?> AuthentificateUser(string email, string password)
        {
            var user = await _unitOfWork.User.GetUserByEmail(email);

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
