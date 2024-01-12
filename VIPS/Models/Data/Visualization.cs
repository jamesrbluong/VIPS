using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.Data
{
    [PrimaryKey(nameof(ContractId), nameof(DeptId), nameof(PartnerId))]
    public class Visualization
    {
        
        public int ContractId { get; set; } 
        
        public int DeptId { get; set; }
        
        public int PartnerId { get; set; }
    }
}
