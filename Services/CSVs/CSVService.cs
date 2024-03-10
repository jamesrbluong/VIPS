using Common.Data;
using Repositories.Contracts;
using Repositories.CSVs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CSVs
{
    public class CSVService : ICSVService
    {
        private readonly ICSVRepository _csvRepository;

        public CSVService(ApplicationDbContext db, ICSVRepository csvRepository)
        {
            _csvRepository = csvRepository;
        }

        public async Task<Common.Entities.CSV> GetById(int contractId, CancellationToken ct)
        {
            return await _csvRepository.GetByIdAsync(contractId, ct);
        }

        public async Task<List<Common.Entities.CSV>> GetCSVsAsync(CancellationToken ct)
        {
            return await _csvRepository.GetListAsync(ct);
        }

    }
}
