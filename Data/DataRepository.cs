using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Friendster.Models;
using Microsoft.EntityFrameworkCore;

namespace Friendster.Data
{
    public class DataRepository : IDataRepository
    {
        private DataContext _context;

        public DataRepository(DataContext context)
        {
            _context = context;

        }

        public async Task<Value> GetValueAsync(int id)
        {
            return await _context.Values.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Value>> GetValuesAsync()
        {
            return await _context.Values.ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
