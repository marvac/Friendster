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
            var sender = await _repo.GetUser(userId);

            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (id != sender.Id)
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
                var messageResource = _mapper.Map<Message, MessageResource>(message);

                return CreatedAtRoute(nameof(GetMessage), new { messageId = message.Id }, messageResource);
            }

            throw new Exception("Failed to save message");
        }

        [HttpPost("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int userId, int messageId)
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

            if (message.SenderId == userId)
            {
                message.SenderDeleted = true;
            }
            else if (message.RecipientId == userId)
            {
                message.RecipientDeleted = true;
            }

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                //both sides have opted to delete the message, remove from database
                _repo.Delete(message);
            }

            if (await _repo.SaveChangesAsync())
            {
                return NoContent();
            }

            throw new Exception("Error deleting message");
        }

        [HttpPost("{messageId}/read")]
        public async Task<IActionResult> MarkAsRead(int userId, int messageId)
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

            if (message.RecipientId != userId)
            {
                return Unauthorized();
            }

            message.MarkedAsRead = true;
            message.ReadDate = DateTime.Now;

            if (await _repo.SaveChangesAsync())
            {
                return NoContent();
            }

            throw new Exception("Error marking message as read");
        }
    }
}
