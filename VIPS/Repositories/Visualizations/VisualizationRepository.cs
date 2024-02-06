using Common.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

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
            return _dbContext.Visualizations.ToListAsync(cancellationToken);
        }

        public async Task<Common.Entities.Visualization> GetByIdAsync(int id)
        {
            return await _dbContext.Visualizations.FindAsync(id);
        }

        public async Task AddAsync(Common.Entities.Visualization visualization, CancellationToken cancellationToken)
        {
            if (visualization != null)
            {
                _dbContext.Visualizations.Add(visualization);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateAsync(Common.Entities.Visualization visualization, CancellationToken cancellationToken)
        {
            if (visualization != null)
            {
                _dbContext.Visualizations.Update(visualization);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var visualizationToDelete = await _dbContext.Visualizations.FindAsync(id);

            if (visualizationToDelete != null)
            {
                _dbContext.Visualizations.Remove(visualizationToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}