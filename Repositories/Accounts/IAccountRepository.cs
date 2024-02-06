using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Entities;

namespace Repositories.Accounts
{
    public interface IAccountRepository
    {
        Task<List<AppUser>> GetListAsync(CancellationToken cancellationToken);
    }
}