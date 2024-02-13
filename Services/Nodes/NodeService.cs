using Common.Entities;
using Repositories.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Nodes
{
    public class NodeService : INodeService
    {
        private readonly INodeRepository _nodeRepository;

        public NodeService(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }
        public async Task<Common.Entities.Node> GetById(string nodeId, CancellationToken ct)
        {
            return await _nodeRepository.GetByIdAsync(nodeId, ct);
        }

        public async Task<List<Common.Entities.Node>> GetNodesAsync(CancellationToken ct)
        {
            return await _nodeRepository.GetListAsync(ct);
        }

        public async Task AddNodeAsync(Node node, CancellationToken ct)
        {
            await _nodeRepository.AddAsync(node, ct);
        }
        public async Task DeleteAllNodes(CancellationToken ct)
        {
            await _nodeRepository.DeleteAllNodes(ct);
        }

    }
}
