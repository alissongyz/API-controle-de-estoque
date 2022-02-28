using estoque_tek.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace estoque_tek.Domains.Interfaces
{
    public interface IProductRepository
    {
        Task<long> CountAsync(string contractorId);

        Task<IEnumerable<Product>> GetAllAsync(string contractorId, string productName, string category, int? page);

        Task<Product> GetOneAsync(string productId);

        Task CreateAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAync(string productId);
    }
}
