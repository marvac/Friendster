using AutoMapper;
using Friendster.Controllers.Resources;
using Friendster.Data;
using Friendster.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

            var user = _mapper.Map<RegisterUserResource, User>(userResource);
            var registeredUser = await _repo.Register(user, userResource.Password);
            var returnUser = _mapper.Map<User, DetailUserResource>(registeredUser);

            return CreatedAtRoute(nameof(UsersController.GetUser), new { controller = nameof(UsersController), userId = registeredUser.Id }, returnUser);
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
