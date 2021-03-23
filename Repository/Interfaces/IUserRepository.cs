using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(string id);
    }
}