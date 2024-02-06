using Common.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Repositories.Visualizations
{
    public class VisualizationRepository : IVisualizationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VisualizationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Common.Entities.Visualization>> GetListAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return _dbContext.Visualizations.ToListAsync(cancellationToken);

            }
            return default;
        }

        public async Task<Common.Entities.Visualization> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _dbContext.Visualizations.FindAsync(id);

            }
            return default;
        }

        public async Task AddAsync(Common.Entities.Visualization visualization, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Visualizations.Add(visualization);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task UpdateAsync(Common.Entities.Visualization visualization, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Visualizations.Update(visualization);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var visualizationToDelete = await _dbContext.Visualizations.FindAsync(id);

            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Visualizations.Remove(visualizationToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }
    }
}