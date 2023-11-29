using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.ViewModels.Account.ForgotPassword
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match, try again!")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ResetCode { get; set; }

    }
}
