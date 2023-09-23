using System.Diagnostics.Contracts;
using VIPS.Models.Data;

namespace VIPS.Repositories
{
    public class FakeRepository
    {
        public IEnumerable<Models.Data.Contract> Contracts => new List<Models.Data.Contract> {
            new Models.Data.Contract { ContractId = 1, Name = "AA - Baytree Behavioral Health, PA"},
            new Models.Data.Contract { ContractId = 2, Name = "AA - Agreement between UNF and University of Neuchatel (UNINE) 2023-2028"},
            new Models.Data.Contract { ContractId = 3, Name = "AA - COEHS - UNF Athletics Department of Recreation and Wellness Letter of Understanding"},
        };

        public IEnumerable<School> Schools => new List<School>
        {
            new School { }
        };

        public IEnumerable<Department> Departments => new List<Department> {
            new Department { DepartmentId = 1, Name = "School of Computing", SchoolId = 1 }
        };
    }
}
