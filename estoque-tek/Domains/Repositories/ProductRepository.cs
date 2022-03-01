using estoque_tek.Domains.Interfaces;
using estoque_tek.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace estoque_tek.Domains.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private const string databaseName = "EstoqueTek";
        private const string collectionName = "products";
        public readonly IMongoCollection<Product> collection;
        private readonly FilterDefinitionBuilder<Product> filterBuilder = Builders<Product>.Filter;

        public ProductRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            collection = database.GetCollection<Product>(collectionName);
        }

        public async Task<long> CountAsync(string contractorId)
        {
            var builder = Builders<Product>.Filter;
            FilterDefinition<Product> filters = builder.Empty;

            if (!string.IsNullOrEmpty(contractorId))
            {
                var name = string.IsNullOrEmpty(contractorId) ? builder.Eq(f => f.ContractorId, contractorId) : builder.Regex(d => d.ContractorId, new BsonRegularExpression(contractorId, "i"));
                filters &= name;
            }

            var find = collection.Find(filters);

            return await find.CountDocumentsAsync();
        }

        public async Task CreateAsync(Product product)
        {
            await collection.InsertOneAsync(product);
        }

        public async Task DeleteAync(string productId)
        {
            var filter = filterBuilder.Eq(c => c.ProductId, productId);

            await collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(string contractorId, string productName, string category, int? page)
        {
            var builder = Builders<Product>.Filter;
            FilterDefinition<Product> filters = builder.Empty;

            if (!string.IsNullOrEmpty(contractorId))
            {
                var contractor = string.IsNullOrEmpty(contractorId) ? builder.Eq(f => f.ContractorId, contractorId) : builder.Regex(d => d.ContractorId, new BsonRegularExpression(contractorId, "i"));
                filters &= contractor;
            }

            if (!string.IsNullOrEmpty(productName))
            {
                var name = string.IsNullOrEmpty(productName) ? builder.Eq(f => f.ProductName, productName) : builder.Regex(d => d.ProductName, new BsonRegularExpression(productName, "i"));
                filters &= name;
            }

            if (!string.IsNullOrEmpty(category))
            {
                var categories = string.IsNullOrEmpty(category) ? builder.Eq(f => f.Category, category) : builder.Regex(d => d.Category, new BsonRegularExpression(category, "i"));
                filters &= categories;
            }

            var find = collection.Find(filters);

            int paGe = page.GetValueOrDefault(1) == 0 ? 1 : page.GetValueOrDefault(1);
            int pageSize = 10;

            return await find.Skip((paGe - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task<Product> GetOneAsync(string productId)
        {
            var filter = filterBuilder.Eq(c => c.ProductId, productId);

            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            var filter = filterBuilder.Eq(c => c.ProductId, product.ProductId);

            await collection.ReplaceOneAsync(filter, product);
        }
    }
}
