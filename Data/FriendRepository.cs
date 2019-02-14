using Friendster.Helpers;
using Friendster.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

            return await PagedList<User>.CreateAsync(users, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }


    }
}
