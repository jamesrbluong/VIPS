using Common.Data;
using Common.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace Repositories.Accounts
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;

        public AccountRepository(Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<List<Common.Entities.AppUser>> GetListAsync(CancellationToken cancellationToken)
        {
            return _userManager.Users.ToListAsync(cancellationToken);
        }

    }
}