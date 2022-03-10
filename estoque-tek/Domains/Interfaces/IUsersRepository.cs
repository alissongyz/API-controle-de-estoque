using estoque_tek.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace estoque_tek.Domains.Interfaces
{
    public interface IUsersRepository
    {
        Task<long> CountAsync(string contractorId);

        Task<IEnumerable<User>> GetAllAsync(string contractorId, string userName, int? page);

        Task<User> GetOneAsync(string userId);

        Task<User> GetOneUserAsync(string contractorId, string userName, string password);

        Task CreateAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAync(string userId);
    }
}
