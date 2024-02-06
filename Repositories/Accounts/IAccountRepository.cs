using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Entities;

namespace Repositories.Accounts
{
    public interface IAccountRepository
    {
        Task<List<AppUser>> GetListAsync(CancellationToken cancellationToken);
        Task<AppUser> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task SignOutAsync(CancellationToken cancellationToken);
        Task PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken ct);
        Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken cancellationToken);
    }
}