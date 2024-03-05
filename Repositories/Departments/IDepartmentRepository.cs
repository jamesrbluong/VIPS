﻿using Common.Entities;

namespace Repositories.Contracts
{
    public interface IDepartmentRepository
    {
        Task AddAsync(Department department, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Department> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<Department>> GetListAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Department department, CancellationToken cancellationToken);
    }
}