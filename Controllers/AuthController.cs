using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Friendster.Controllers.Resources;
using Friendster.Data;
using Friendster.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Friendster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _repo;
        private IConfiguration _config;
        private IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserResource userResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (await _repo.UserExists(userResource.Username))
            {
                return BadRequest("Username already exists");
            }

            User user = new User
            {
                Username = userResource.Username
            };

            var registeredUser = await _repo.Register(user, userResource.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserResource loginUserResource)
        {
            var user = await _repo.Login(loginUserResource.Username, loginUserResource.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            /* User is authenticated */

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userResource = _mapper.Map<User, ListUserResource>(user);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                userResource
            });
        }
    }
}
