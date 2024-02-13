using Common.Data;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Edges
{
    public class EdgeRepository : IEdgeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EdgeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Common.Entities.Edge>> GetListAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return _dbContext.Edges.ToListAsync(cancellationToken);

            }
            return default;
        }

        public async Task<Common.Entities.Edge> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _dbContext.Edges.FindAsync(id);

            }
            return default;
        }

        public async Task AddAsync(Common.Entities.Edge visualization, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Edges.Add(visualization);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task UpdateAsync(Common.Entities.Edge visualization, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Edges.Update(visualization);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var edgeToDelete = await _dbContext.Edges.FindAsync(id);

            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Edges.Remove(edgeToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }
    }
}
