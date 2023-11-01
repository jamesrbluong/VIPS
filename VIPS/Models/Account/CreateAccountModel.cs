using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.Account
{
    public class CreateAccountModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
