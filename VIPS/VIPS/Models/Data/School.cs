using System.ComponentModel.DataAnnotations;

namespace VIPS.Models.Data
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public virtual IEnumerable<Department> Departments { get; set; } = new List<Department>();

    }
}
