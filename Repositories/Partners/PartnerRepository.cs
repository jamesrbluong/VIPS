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
            return _dbContext.Partners.ToListAsync(cancellationToken);
        }

        public async Task<Common.Entities.Partner> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Partners.FindAsync(id);
        }

        public async Task AddAsync(Common.Entities.Partner partner, CancellationToken cancellationToken)
        {
            _dbContext.Partners.Add(partner);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Common.Entities.Partner partner, CancellationToken cancellationToken)
        {
            _dbContext.Partners.Update(partner);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var partnerToDelete = await _dbContext.Partners.FindAsync(id);
            _dbContext.Partners.Remove(partnerToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
