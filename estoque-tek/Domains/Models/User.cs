using System.ComponentModel.DataAnnotations;

namespace estoque_tek.Models
{
    public class User
    {
        public string UserId { get; set; }

        public string ContractorId { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}