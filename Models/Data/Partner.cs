using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.Data
{
    public class Partner
    {
        [Key]
        public int PartnerId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
