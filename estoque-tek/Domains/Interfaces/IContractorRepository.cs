using estoque_tek.Models;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace estoque_tek.Domains.Interfaces
{
    public interface IContractorRepository
    {
        Task<long> CountAsync();

        Task<IEnumerable<Contractor>> GetAllAsync(string displayName, string cnpj, int? page);

        Task<Contractor> GetOneAsync(string contractorId);

        Task CreateAsync(Contractor contractor);

        Task UpdateAsync(Contractor contractor);

        Task DeleteAync(string contractorId);
    }
}
