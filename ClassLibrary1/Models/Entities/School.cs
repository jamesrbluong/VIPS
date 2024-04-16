using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VIPS.Common.Models.Entities
{
    public class School
    {
        public School()
        {
            Departments = Enumerable.Empty<Department>();
        }

        [Key]
        public int SchoolId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public virtual IEnumerable<Department> Departments { get; set; }
    }
}
