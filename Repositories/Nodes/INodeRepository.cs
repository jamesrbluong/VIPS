using Common.Entities;

namespace Repositories.Nodes
{
    public interface INodeRepository
    {
        Task AddAsync(Node visualization, CancellationToken cancellationToken);
        Task DeleteAllNodes(CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Node> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<List<Node>> GetListAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Node visualization, CancellationToken cancellationToken);
    }
}