using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.Data
{
    [PrimaryKey(nameof(ContractId), nameof(DeptId), nameof(PartnerId))]
    public class Visualization
    {
        
        public string ContractId { get; set; } = string.Empty;
        
        public string DeptId { get; set; } = string.Empty;
        
        public string PartnerId { get; set; } = string.Empty;
    }
}
