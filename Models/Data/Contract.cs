using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.Data
{
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
