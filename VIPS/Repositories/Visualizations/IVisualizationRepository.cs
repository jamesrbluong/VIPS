using Common.Entities;

namespace Repositories.Visualizations
{
    public interface IVisualizationRepository
    {
        Task<List<Visualization>> GetListAsync(CancellationToken cancellationToken);
        Task<Common.Entities.Visualization> GetByIdAsync(int id);
        Task AddAsync(Common.Entities.Visualization visualization, CancellationToken cancellationToken);
        Task UpdateAsync(Common.Entities.Visualization visualization, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}