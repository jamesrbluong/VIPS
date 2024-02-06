using Common.Data;
using Repositories.Contracts;
using Repositories.Schools;
using Services.Schools;


namespace Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(ApplicationDbContext db, IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Common.Entities.Department> GetById(int departmentId)
        {
            return await _departmentRepository.GetByIdAsync(departmentId);
        }

        public async Task<object> GetBySchool(int SchoolId, CancellationToken ct)
        {
            var depts = await GetDepartmentsAsync(ct);
            var deptNames = depts.Where(x => x.SchoolId == SchoolId).Select(x => new { departmentName = x.Name }).ToList();

            return deptNames;
        }

        public async Task<List<Common.Entities.Department>> GetDepartmentsAsync(CancellationToken ct)
        {
            return await _departmentRepository.GetListAsync(ct);
        }

        


    }
}
