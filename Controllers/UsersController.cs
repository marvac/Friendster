using AutoMapper;
using Friendster.Controllers.Resources;
using Friendster.Data;
using Friendster.Helpers;
using Friendster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Friendster.Controllers
{
    [ServiceFilter(typeof(LogActivity))]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IFriendRepository _repo;
        private IMapper _mapper;

        public UsersController(IFriendRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var usersResource = _mapper.Map<IEnumerable<User>, IEnumerable<ListUserResource>>(users);
            return Ok(usersResource);
        }

        [HttpGet("{userId}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _repo.GetUser(userId);
            var userResource = _mapper.Map<User, DetailUserResource>(user);
            return Ok(userResource);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserResource userResource)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (userId != id)
            {
                return Unauthorized();
            }

            var user = await _repo.GetUser(userId);
            _mapper.Map(userResource, user);

            if (await _repo.SaveChangesAsync())
            {
                return NoContent();
            }

            throw new Exception($"Updating user ID {userId} failed to save");
        }
    }
}
