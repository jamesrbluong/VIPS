using Common.Data;
using Repositories.Departments;
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

        public async Task<Common.Entities.Department> GetById(int departmentId, CancellationToken ct)
        {
            return await _departmentRepository.GetByIdAsync(departmentId, ct);
        }

        public async Task<List<Common.Entities.Department>> GetDepartmentsAsync(CancellationToken ct)
        {
            return await _departmentRepository.GetListAsync(ct);
        }

        


    }
}
