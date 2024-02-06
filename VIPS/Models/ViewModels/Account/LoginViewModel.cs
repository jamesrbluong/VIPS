using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.ViewModels.Account
{
    public class LoginViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; } = "/";


    }
}
