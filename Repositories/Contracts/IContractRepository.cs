using Common.Entities;

namespace Repositories.Contracts
{
    public interface IContractRepository
    {
        Task AddAsync(Contract contract, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Contract> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<Contract>> GetListAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Contract contract, CancellationToken cancellationToken);
    }
}