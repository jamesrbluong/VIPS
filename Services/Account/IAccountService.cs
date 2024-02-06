using Common.Entities;

namespace Services.Account
{
    public interface IAccountService
    {
        Task<List<AppUser>> GetAccountsAsync(CancellationToken ct);
        Task<AppUser> GetByEmailAsync(string email, CancellationToken ct);
        Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken ct);
        Task PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken ct);
        Task SignOutAsync(CancellationToken ct);
    }
}