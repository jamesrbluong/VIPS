using Common.Data;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Nodes
{
    public class NodeRepository : INodeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NodeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Common.Entities.Node>> GetListAsync(CancellationToken cancellationToken)
        {
            return _dbContext.Nodes.ToListAsync(cancellationToken);
        }

        public async Task<Common.Entities.Node> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _dbContext.Nodes.FindAsync(id);
        }

        public async Task AddAsync(Common.Entities.Node visualization, CancellationToken cancellationToken)
        {
            _dbContext.Nodes.Add(visualization);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Common.Entities.Node visualization, CancellationToken cancellationToken)
        {
            _dbContext.Nodes.Update(visualization);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var nodeToDelete = await _dbContext.Nodes.FindAsync(id);
            _dbContext.Nodes.Remove(nodeToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAllNodes(CancellationToken cancellationToken)
        {
            var data = await _dbContext.Nodes.ToListAsync(cancellationToken);
            _dbContext.Nodes.RemoveRange(data);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
