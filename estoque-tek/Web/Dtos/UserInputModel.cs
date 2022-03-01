using System.ComponentModel.DataAnnotations;

namespace estoque_tek.Web.Dtos
{
    public class UserInputModel
    {
        public string ContractorId { get; set; }

        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
