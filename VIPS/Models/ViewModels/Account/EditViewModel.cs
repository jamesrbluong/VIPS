using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.ViewModels.Account
{
    public class EditViewModel
    {
        [Required]
        public string Id { get; set; }
        public string SecurityStamp { get; set; }
        public string RoleName { get; set; }
        
    }
}
