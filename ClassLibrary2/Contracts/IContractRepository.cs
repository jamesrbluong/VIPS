using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VIPS.Common.Models.Entities;

namespace VIPS.Repositories.Contracts
{
    public interface IContractRepository
    {
        Task<List<Contract>> GetListAsync(CancellationToken cancellationToken);
    }
}
