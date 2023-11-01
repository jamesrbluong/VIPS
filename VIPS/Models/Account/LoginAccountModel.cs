using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.Account
{
    public class LoginAccountModel
    {
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; } = "/";


    }
}
