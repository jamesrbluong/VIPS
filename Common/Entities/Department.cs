using System.ComponentModel.DataAnnotations;

namespace Common.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SchoolId { get; set; } // Departments are subsets of the school (ex. School of computing belongs to CCEC)
    }
}
