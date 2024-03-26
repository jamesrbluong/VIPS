using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Edges
{
    public interface IEdgeService
    {
        Task<Edge> GetById(int edgeId, CancellationToken ct);
        Task<List<Edge>> GetEdgesAsync(CancellationToken ct);
    }
}
