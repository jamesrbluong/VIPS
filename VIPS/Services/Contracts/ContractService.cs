using Common.Data;
using Common.Entities;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;

        public ContractService(ApplicationDbContext db, IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<Common.Entities.Contract> GetById(int contractId)
        {
            return await _contractRepository.GetByIdAsync(contractId);
        }

        public async Task<List<Common.Entities.Contract>> GetContractsAsync (CancellationToken ct)
        {
            return await _contractRepository.GetListAsync(ct);
        }

        
    }
}
