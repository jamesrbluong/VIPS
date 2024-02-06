using Common.Entities;

namespace Services.Departments
{
    public interface IDepartmentService
    {
        Task<Department> GetById(int departmentId);
        Task<object> GetBySchool(int SchoolId, CancellationToken ct);
        Task<List<Department>> GetDepartmentsAsync(CancellationToken ct);
    }
}