using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.ViewModels.Account
{
    public class CreateViewModel
    {
        
        [Required]
        [EmailAddress]
        // [RegularExpression(@"^[a-zA-Z0-9._%+-]+(@unf.edu)$", ErrorMessage = "Registration limited to @unf.edu.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
