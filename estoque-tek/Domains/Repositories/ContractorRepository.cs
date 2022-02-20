using estoque_tek.Domains.Interfaces;
using estoque_tek.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace estoque_tek.Domains.Repositories
{
    public class ContractorRepository : IContractorRepository
    {
        private const string databaseName = "EstoqueTek";
        private const string collectionName = "contractors";
        public readonly IMongoCollection<Contractor> collection;
        private readonly FilterDefinitionBuilder<Contractor> filterBuilder = Builders<Contractor>.Filter;

        public ContractorRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            collection = database.GetCollection<Contractor>(collectionName);
        }

        public async Task<long> CountAsync()
        {
            return await collection.Find(new BsonDocument()).CountDocumentsAsync();
        }

        public async Task CreateAsync(Contractor contractor)
        {
            await collection.InsertOneAsync(contractor);
        }

        public async Task DeleteAync(string contractorId)
        {
            var filter = filterBuilder.Eq(c => c.ContractorId, contractorId);
            await collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Contractor>> GetAllAsync(string displayName, string cnpj, int? page)
        {
            var builder = Builders<Contractor>.Filter;
            FilterDefinition<Contractor> filters = builder.Empty;

            if (!string.IsNullOrEmpty(displayName))
            {
                var name = string.IsNullOrEmpty(displayName) ? builder.Eq(f => f.DisplayName, displayName) : builder.Regex(d => d.DisplayName, new BsonRegularExpression(displayName, "i"));
                filters &= name;
            }

            if (!string.IsNullOrEmpty(cnpj))
            {
                var cnpJ = string.IsNullOrEmpty(cnpj) ? builder.Eq(f => f.Cnpj, displayName) : builder.Regex(d => d.Cnpj, new BsonRegularExpression(cnpj, "i"));
                filters &= cnpJ;
            }

            var find = collection.Find(filters);

            int paGe = page.GetValueOrDefault(1) == 0 ? 1 : page.GetValueOrDefault(1);
            int pageSize = 10;

            return await find.Skip((paGe - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task<Contractor> GetOneAsync(string ContractorId)
        {
            var filter = filterBuilder.Eq(c => c.ContractorId, ContractorId);
            return await collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(Contractor contractor)
        {
            var filter = filterBuilder.Eq(c => c.ContractorId, contractor.ContractorId);
            await collection.ReplaceOneAsync(filter, contractor);
        }
    }
}
