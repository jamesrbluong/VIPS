using Common.Data;
using Common.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace Repositories.Departments
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
            return _dbContext.Departments.ToListAsync(cancellationToken);
        }

        public async Task<Common.Entities.Department> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Departments.FindAsync(id, cancellationToken);
        }

        public async Task<Department> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Departments.Where(d => d.Name.Equals(name)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(Common.Entities.Department department, CancellationToken cancellationToken)
        {
            _dbContext.Departments.Add(department);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Common.Entities.Department department, CancellationToken cancellationToken)
        {
            _dbContext.Departments.Update(department);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var departmentToDelete = await _dbContext.Departments.FindAsync(id);

            _dbContext.Departments.Remove(departmentToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
