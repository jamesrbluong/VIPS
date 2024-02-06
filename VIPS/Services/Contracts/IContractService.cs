using Common.Entities;

namespace Services.Contracts
{
    public interface IContractService
    {
        public Task<Contract> GetById(int contractId);
        public Task<List<Common.Entities.Contract>> GetContractsAsync(CancellationToken ct);
    }
}