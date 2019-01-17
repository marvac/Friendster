﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Friendster.Controllers.Resources;
using Friendster.Data;
using Friendster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Friendster.Controllers
{
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _repo.GetUser(userId);
            var userResource = _mapper.Map<User, DetailUserResource>(user);
            return Ok(userResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserResource userResource)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (id != userId)
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
