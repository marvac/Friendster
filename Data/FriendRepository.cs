using Friendster.Helpers;
using Friendster.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friendster.Data
{
    public class FriendRepository : IFriendRepository
    {
        private DataContext _context;

        public FriendRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int userId)
        {
            var user = await _context.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Id == userId);

            return user;
        }

        public async Task<Photo> GetPhoto(int photoId)
        {
            return await _context.Photos.FirstOrDefaultAsync(x => x.Id == photoId);
        }

        public async Task<Photo> GetMainPhoto(int userId)
        {
            return await _context.Photos.Where(x => x.UserId == userId)
                 .FirstOrDefaultAsync(x => x.IsMain);
        }

        public async Task<PagedList<User>> GetUsers(UserParameters parameters)
        {
            var minimumBirthDate = DateTime.Now.AddYears(-parameters.MaximumAge -1);
            var maximumBirthDate = DateTime.Now.AddYears(-parameters.MinimumAge);

            var users = _context.Users
                .Include(p => p.Photos)
                .Where(u => u.Id != parameters.UserId &&
                    u.Gender == parameters.Gender &&
                    u.BirthDate >= minimumBirthDate &&
                    u.BirthDate <= maximumBirthDate);

            if (parameters.Likers)
            {
                var likers = await GetUserLikes(parameters.UserId, true);
                users = users.Where(u => likers.Contains(u.Id));
            }

            if (parameters.Likees)
            {
                var likees = await GetUserLikes(parameters.UserId, false);
                users = users.Where(u => likees.Contains(u.Id));
            }

            string orderBy = parameters.OrderBy?.ToLower();
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                switch (orderBy)
                {
                    case "created":
                        users = users.OrderByDescending(x => x.DateCreated);
                        break;
                    default:
                        users = users.OrderByDescending(x => x.LastActive);
                        break;
                }
            }
            
            return await PagedList<User>.CreateAsync(users, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(x => x.LikerId == userId && x.LikeeId == recipientId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<IEnumerable<int>> GetUserLikes(int userId, bool getLikers)
        {
            var user = await _context.Users
                .Include(x => x.Likers)
                .Include(x => x.Likees)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (getLikers)
            {
                return user.Likers
                    .Where(u => u.LikeeId == userId)
                    .Select(u => u.LikerId);
            }

            return user.Likees
                .Where(u => u.LikerId == userId)
                .Select(u => u.LikeeId);
        }

        public async Task<Message> GetMessage(int messageId)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<PagedList<Message>> GetMessages(MessageParameters messageParameters)
        {
            var messages = _context.Messages
                .Include(u => u.Sender)
                .ThenInclude(p => p.Photos)
                .Include(u => u.Recipient)
                .ThenInclude(p => p.Photos)
                .AsQueryable();

            switch (messageParameters.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParameters.UserId && !u.RecipientDeleted);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParameters.UserId && !u.SenderDeleted);
                    break;
                default: //just return unread messages
                    messages = messages.Where(u => u.RecipientId == messageParameters.UserId && !u.MarkedAsRead && !u.RecipientDeleted);
                    break;
            }

            messages = messages.OrderByDescending(m => m.MessageSent);
            return await PagedList<Message>.CreateAsync(messages, messageParameters.PageNumber, messageParameters.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages
                .Include(u => u.Sender)
                .ThenInclude(p => p.Photos)
                .Include(u => u.Recipient)
                .ThenInclude(p => p.Photos)
                .Where(m => 
                m.RecipientId == userId && 
                !m.RecipientDeleted &&
                m.SenderId == recipientId || 
                m.RecipientId == recipientId && 
                m.SenderId == userId &&
                !m.SenderDeleted)
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();

            return messages;
        }
    }
}
