using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Nodes
{
    public interface INodeService
    {
        Task AddNodeAsync(Node node, CancellationToken ct);
        Task DeleteAllNodes(CancellationToken ct);
        Task<Node> GetById(string nodeId, CancellationToken ct);
        Task<List<Node>> GetNodesAsync(CancellationToken ct);
    }
}
