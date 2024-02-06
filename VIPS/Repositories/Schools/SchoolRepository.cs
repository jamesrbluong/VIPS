using Common.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace Repositories.Schools
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SchoolRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Common.Entities.School>> GetListAsync(CancellationToken cancellationToken)
        {
            return _dbContext.Schools.ToListAsync(cancellationToken);
        }

        public async Task<Common.Entities.School> GetByIdAsync(int id)
        {
            return await _dbContext.Schools.FindAsync(id);
        }

        public async Task AddAsync(Common.Entities.School school, CancellationToken cancellationToken)
        {
            if (school != null)
            {
                _dbContext.Schools.Add(school);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateAsync(Common.Entities.School school, CancellationToken cancellationToken)
        {
            if (school != null)
            {
                _dbContext.Schools.Update(school);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var schoolToDelete = await _dbContext.Schools.FindAsync(id);

            if (schoolToDelete != null)
            {
                _dbContext.Schools.Remove(schoolToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

    }
}