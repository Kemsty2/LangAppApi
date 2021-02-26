using System.ComponentModel.DataAnnotations;

namespace LangAppApi.Domain.Auth
{
    public class AuthenticationRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}