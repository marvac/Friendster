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
        public async Task<IActionResult> GetUsers([FromQuery]UserParameters parameters)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var currentUser = await _repo.GetUser(currentUserId);
            parameters.UserId = currentUserId;

            if (string.IsNullOrWhiteSpace(parameters.Gender))
            {
                parameters.Gender = currentUser.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(parameters);
            var usersResource = _mapper.Map<IEnumerable<User>, IEnumerable<ListUserResource>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalItems, users.TotalPages);

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

        [HttpPost("{userId}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int userId, int recipientId)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var like = await _repo.GetLike(userId, recipientId);

            if (like != null)
            {
                return BadRequest("This person was already liked by you");
            }

            if (_repo.GetUser(recipientId) == null)
            {
                return NotFound("The user you tried to like doesn't exist");
            }

            like = new Like
            {
                LikerId = userId,
                LikeeId = recipientId
            };

            _repo.Add(like);

            if (await _repo.SaveChangesAsync())
            {
                return Ok(like);
            }

            return BadRequest("Failed to like this person");
        }
    }
}
