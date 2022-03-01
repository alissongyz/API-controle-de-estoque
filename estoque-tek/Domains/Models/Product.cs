using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace estoque_tek.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }
        public string ContractorId { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int MinimumStock { get; set; }

        public int MaximumStock { get; set; }

        public double CostValue { get; set; }

        public double UnitaryValue { get; set; }
    }
}
