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
            while (!cancellationToken.IsCancellationRequested)
            {
                return _dbContext.Nodes.ToListAsync(cancellationToken);

            }
            return default;
        }

        public async Task<Common.Entities.Node> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _dbContext.Nodes.FindAsync(id);

            }
            return default;
        }

        public async Task AddAsync(Common.Entities.Node visualization, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Nodes.Add(visualization);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task UpdateAsync(Common.Entities.Node visualization, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Nodes.Update(visualization);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var nodeToDelete = await _dbContext.Nodes.FindAsync(id);

            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Nodes.Remove(nodeToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task DeleteAllNodes(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var data = _dbContext.Nodes.ToList();
                _dbContext.Nodes.RemoveRange(data);
                await _dbContext.SaveChangesAsync();

                return;
            }
        }
    }
}
