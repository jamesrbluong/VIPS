using Common.Entities;

namespace Repositories.Edges
{
    public interface IEdgeRepository
    {
        Task AddAsync(Edge visualization, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Edge> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<List<Edge>> GetListAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Edge visualization, CancellationToken cancellationToken);
    }
}