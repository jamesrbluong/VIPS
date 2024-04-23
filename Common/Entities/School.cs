using System.ComponentModel.DataAnnotations;

namespace Common.Entities
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
