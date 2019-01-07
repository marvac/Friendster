using Friendster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friendster.Data
{
    public interface IDataRepository
    {
        Task<int> SaveChangesAsync();
        Task<IEnumerable<Value>> GetValuesAsync();
        Task<Value> GetValueAsync(int id);
    }
}
