using Friendster.Models;
using System.Collections.Generic;
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
