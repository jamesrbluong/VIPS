using Common.Entities;

namespace Services.Departments
{
    public interface IDepartmentService
    {
        Task<Common.Entities.Department> GetById(int departmentId, CancellationToken ct);
        Task<List<Department>> GetDepartmentsAsync(CancellationToken ct);
    }
}