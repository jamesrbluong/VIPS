using System.ComponentModel.DataAnnotations;

namespace Common.Entities
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }
        public string Name { get; set; } = string.Empty;
        /*Changed the table in the database so these variables are no longer necessary*/
/*        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public virtual IEnumerable<Department> Departments { get; set; } = new List<Department>();*/

    }
}
