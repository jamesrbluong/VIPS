using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Entities;

namespace Repositories.Contracts
{
    public interface IContractRepository
    {
        Task<List<Common.Entities.Contract>> GetListAsync(CancellationToken cancellationToken);
        Task<Common.Entities.Contract> GetByIdAsync(int id);
        Task AddAsync(Common.Entities.Contract contract, CancellationToken cancellationToken);
        Task UpdateAsync(Common.Entities.Contract contract, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
}