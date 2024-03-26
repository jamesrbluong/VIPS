using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Common.Entities
{
    [PrimaryKey(nameof(ContractId), nameof(FromId), nameof(ToId))]
    public class Edge
    {
        public int ContractId { get; set; } 
        public string FromId { get; set; }
        public string ToId { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
