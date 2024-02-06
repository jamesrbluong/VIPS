using Common.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace Repositories.Partners
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PartnerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Common.Entities.Partner>> GetListAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return _dbContext.Partners.ToListAsync(cancellationToken);
            }

            return default;
        }

        public async Task<Common.Entities.Partner> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _dbContext.Partners.FindAsync(id);
            }

            return default;
        }

        public async Task AddAsync(Common.Entities.Partner partner, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Partners.Add(partner);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task UpdateAsync(Common.Entities.Partner partner, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Partners.Update(partner);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var partnerToDelete = await _dbContext.Partners.FindAsync(id);

            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Partners.Remove(partnerToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }
    }
}