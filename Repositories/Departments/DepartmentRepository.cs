using Common.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace Repositories.Contracts
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Common.Entities.Department>> GetListAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return _dbContext.Departments.ToListAsync(cancellationToken);
            }

            return default;
        }

        public async Task<Common.Entities.Department> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _dbContext.Departments.FindAsync(id);
            }
            return default;
        }

        public async Task AddAsync(Common.Entities.Department department, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Departments.Add(department);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task UpdateAsync(Common.Entities.Department department, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Departments.Update(department);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var departmentToDelete = await _dbContext.Departments.FindAsync(id);

            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Departments.Remove(departmentToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }
    }
}