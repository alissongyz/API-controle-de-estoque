using System.ComponentModel.DataAnnotations;

namespace estoque_tek.Web.Dtos
{
    public class ContractorInputModel
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string Cnpj { get; set; }

        [Required]
        public string CorporateName { get; set; }

        [Required(ErrorMessage = "E-mail deve ser preenchido!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha deve ser preenchida!")]
        public string Password { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [MaxLength(2, ErrorMessage = "Formato de UF inválido, preencha um válido, exemplo: SP<br>RJ<br>ES<br>BA")]
        public string Uf { get; set; }
    }
}
