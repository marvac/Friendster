using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Friendster.Controllers.Resources;
using Friendster.Data;
using Friendster.Models;
using Microsoft.AspNetCore.Mvc;

namespace Friendster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserResource userResource)
        {
            if (string.IsNullOrWhiteSpace(userResource?.UserName) || string.IsNullOrWhiteSpace(userResource?.Password))
            {
                return BadRequest("Username or password cannot be blank");
            }

            if (await _repo.UserExists(userResource.UserName))
            {
                return BadRequest("Username already exists");
            }

            User user = new User
            {
                Username = userResource.UserName
            };

            var registeredUser = await _repo.Register(user, userResource.Password);

            return StatusCode(201);
        }
    }
}
