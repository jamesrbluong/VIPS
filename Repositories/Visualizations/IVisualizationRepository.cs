using Common.Entities;

namespace Repositories.Visualizations
{
    public interface IVisualizationRepository
    {
        Task AddAsync(Visualization visualization, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Visualization> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<List<Visualization>> GetListAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Visualization visualization, CancellationToken cancellationToken);
    }
}