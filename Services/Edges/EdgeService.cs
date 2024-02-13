using Repositories.Edges;
using Services.Departments;
using Services.Partners;
using Services.Schools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Edges
{
    public class EdgeService : IEdgeService
    {
        private readonly IEdgeRepository _edgeRepository;

        public EdgeService(IEdgeRepository edgeRepository)
        {
            _edgeRepository = edgeRepository;
        }

        public async Task<Common.Entities.Edge> GetById(int edgeId, CancellationToken ct)
        {
            return await _edgeRepository.GetByIdAsync(edgeId, ct);
        }

        public async Task<List<Common.Entities.Edge>> GetEdgesAsync(CancellationToken ct)
        {
            return await _edgeRepository.GetListAsync(ct);
        }

    }
}
