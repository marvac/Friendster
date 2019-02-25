using Friendster.Helpers;
using Friendster.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Friendster.Data
{
    public interface IFriendRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<PagedList<User>> GetUsers(UserParameters parameters);
        Task<User> GetUser(int userId);
        Task<Photo> GetPhoto(int photoId);
        Task<Photo> GetMainPhoto(int userId);
        Task<Like> GetLike(int userId, int recipientId);
        Task<Message> GetMessage(int messageId);
        Task<PagedList<Message>> GetMessages(MessageParameters messageParameters);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}
