using System.ComponentModel.DataAnnotations;

namespace estoque_tek.Models
{
    public class Contractor
    {
        public string Id { get; set; }

        public string ContractorId { get; set; }

        public string DisplayName { get; set; }

        public string Cnpj { get; set; }

        public string CorporateName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public string City { get; set; }

        public string Uf { get; set; }

        public string status { get; set; }
    }
}
