using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.ViewModels.Account
{
    public class EditViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string SecurityStamp { get; set; }
        public string RoleName { get; set; }
        
    }
}
