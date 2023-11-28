using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.ViewModels.Account.ForgotPassword
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

    }
}
