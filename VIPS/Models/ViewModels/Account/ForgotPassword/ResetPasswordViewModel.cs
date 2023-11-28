using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.ViewModels.Account.ForgotPassword
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ResetCode { get; set; }

    }
}
