using System.ComponentModel.DataAnnotations;

namespace Common.Entities
{
    public class Partner
    {
        [Key]
        public int PartnerId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
