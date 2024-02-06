using Common.Entities;

namespace Repositories.Contracts
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetListAsync(CancellationToken cancellationToken);
        Task<Common.Entities.Department> GetByIdAsync(int id);
        Task AddAsync(Common.Entities.Department department, CancellationToken cancellationToken);
        Task UpdateAsync(Common.Entities.Department department, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
}