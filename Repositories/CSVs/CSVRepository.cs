using Common.Data;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CSVs
{

    public class CSVRepository : ICSVRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CSVRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Common.Entities.CSV>> GetListAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return _dbContext.CSVs.ToListAsync(cancellationToken);
            }

            return default;
        }

        public async Task<Common.Entities.CSV> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _dbContext.CSVs.FindAsync(id);
            }

            return default;

        }

        public async Task AddAsync(Common.Entities.CSV csv, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.CSVs.Add(csv);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var csvToDelete = await _dbContext.CSVs.FindAsync(id);

                _dbContext.CSVs.Remove(csvToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task UpdateAsync(Common.Entities.CSV csv, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.CSVs.Update(csv);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }
    }
}
