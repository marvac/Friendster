using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Friendster.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();

            return users;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
