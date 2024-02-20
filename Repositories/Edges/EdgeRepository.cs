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
            return _dbContext.Edges.ToListAsync(cancellationToken);
        }

        public async Task<Common.Entities.Edge> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Edges.FindAsync(id);
        }

        public async Task AddAsync(Common.Entities.Edge visualization, CancellationToken cancellationToken)
        {
            _dbContext.Edges.Add(visualization);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Common.Entities.Edge visualization, CancellationToken cancellationToken)
        {
            _dbContext.Edges.Update(visualization);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var edgeToDelete = await _dbContext.Edges.FindAsync(id);

            _dbContext.Edges.Remove(edgeToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
