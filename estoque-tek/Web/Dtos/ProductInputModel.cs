namespace estoque_tek.Web.Dtos
{
    public class ProductInputModel
    {
        public string ContractorId { get; set; }

        public string Category { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public int MinimumStock { get; set; }

        public int MaximumStock { get; set; }

        public double CostValue { get; set; }

        public double ProductValue { get; set; }
    }
}
