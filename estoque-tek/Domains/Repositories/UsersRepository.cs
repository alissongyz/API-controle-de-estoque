using estoque_tek.Domains.Interfaces;
using estoque_tek.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace estoque_tek.Domains.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private const string databaseName = "EstoqueTek";
        private const string collectionName = "users";
        public readonly IMongoCollection<User> collection;
        private readonly FilterDefinitionBuilder<User> filterBuilder = Builders<User>.Filter;

        public UsersRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            collection = database.GetCollection<User>(collectionName);
        }

        public async Task<long> CountAsync(string contractorId)
        {
            var builder = Builders<User>.Filter;
            FilterDefinition<User> filters = builder.Empty;

            if (!string.IsNullOrEmpty(contractorId))
            {
                var name = string.IsNullOrEmpty(contractorId) ? builder.Eq(f => f.ContractorId, contractorId) : builder.Regex(d => d.ContractorId, new BsonRegularExpression(contractorId, "i"));
                filters &= name;
            }

            var find = collection.Find(filters);

            return await find.CountDocumentsAsync();
        }

        public async Task CreateAsync(User user)
        {
            await collection.InsertOneAsync(user);
        }

        public async Task DeleteAync(string userId)
        {
            var filter = filterBuilder.Eq(user => user.UserId, userId);
            await collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<User>> GetAllAsync(string contractorId, string userName, int? page)
        {
            var builder = Builders<User>.Filter;
            FilterDefinition<User> filters = builder.Empty;

            if (!string.IsNullOrEmpty(contractorId))
            {
                var name = string.IsNullOrEmpty(contractorId) ? builder.Eq(f => f.ContractorId, contractorId) : builder.Regex(d => d.ContractorId, new BsonRegularExpression(contractorId, "i"));
                filters &= name;
            }

            if (!string.IsNullOrEmpty(userName))
            {
                var name = string.IsNullOrEmpty(userName) ? builder.Eq(f => f.UserName, userName) : builder.Regex(d => d.UserName, new BsonRegularExpression(userName, "i"));
                filters &= name;
            }

            var find = collection.Find(filters);

            int paGe = page.GetValueOrDefault(1) == 0 ? 1 : page.GetValueOrDefault(1);
            int pageSize = 10;

            return await find.Skip((paGe - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task<User> GetOneAsync(string userId)
        {
            var filter = filterBuilder.Eq(user => user.UserId, userId);
            return await collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var filter = filterBuilder.Eq(user => user.UserId, user.UserId);
            await collection.ReplaceOneAsync(filter, user);
        }

        public async Task<User> GetOneUserAsync(string contractorId, string userName, string password)
        {
            var filter = filterBuilder.Where(x => x.ContractorId == contractorId && x.UserName == userName && x.Password == password);

            return await collection.Find(filter).SingleOrDefaultAsync();
        }
    }
}
