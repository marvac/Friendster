using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Friendster.Controllers.Resources;
using Friendster.Data;
using Friendster.Helpers;
using Friendster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Friendster.Controllers
{
    [ServiceFilter(typeof(LogActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private IFriendRepository _repo;
        private IMapper _mapper;

        public MessagesController(IFriendRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{messageId}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int messageId)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (id != userId)
            {
                return Unauthorized();
            }

            var message = await _repo.GetMessage(messageId);
            if (message == null)
            {
                return NotFound();
            }

            var messageResource = _mapper.Map<Message, SendMessageResource>(message);

            return Ok(messageResource);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(int userId, [FromQuery]MessageParameters messageParameters)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (id != userId)
            {
                return Unauthorized();
            }

            messageParameters.UserId = userId;

            var messages = await _repo.GetMessages(messageParameters);

            var messagesResource = _mapper.Map<PagedList<Message>, IEnumerable<Message>>(messages);

            Response.AddPagination(messages.CurrentPage, messages.PageSize, messages.TotalItems, messages.TotalPages);

            return Ok(messagesResource);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (id != userId)
            {
                return Unauthorized();
            }

            var messages = await _repo.GetMessageThread(userId, recipientId);

            var messagesResource = _mapper.Map<IEnumerable<Message>, IEnumerable<MessageResource>>(messages);

            return Ok(messagesResource);
        }

        [HttpPost]
        public async Task<ActionResult> SendMessage(int userId, SendMessageResource sendMessageResource)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (id != userId)
            {
                return Unauthorized();
            }

            sendMessageResource.SenderId = userId;
            var recipient = await _repo.GetUser(sendMessageResource.RecipientId);
            if (recipient == null)
            {
                return BadRequest();
            }

            var message = _mapper.Map<SendMessageResource, Message>(sendMessageResource);
            _repo.Add(message);
            if (await _repo.SaveChangesAsync())
            {
                return CreatedAtRoute(nameof(GetMessage), new { messageId = message.Id }, message);
            }

            throw new Exception("Failed to save message");
        }

        //[HttpDelete("{messageId}")]
        //public async Task<IActionResult> DeleteMessage(int userId, int messageId)
        //{
        //    int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //    if (id != userId)
        //    {
        //        return Unauthorized();
        //    }


        //}
    }
}
