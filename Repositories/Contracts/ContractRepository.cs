using Common.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace Repositories.Contracts
{
    public class ContractRepository : IContractRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ContractRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Common.Entities.Contract>> GetListAsync(CancellationToken cancellationToken)
        {
            return _dbContext.Contracts.ToListAsync(cancellationToken);
        }

        public async Task<Common.Entities.Contract> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Contracts.FindAsync(id);
        }

        public async Task AddAsync(Common.Entities.Contract contract, CancellationToken cancellationToken)
        {
            _dbContext.Contracts.Add(contract);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Common.Entities.Contract contract, CancellationToken cancellationToken)
        {
            _dbContext.Contracts.Update(contract);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var contractToDelete = await _dbContext.Contracts.FindAsync(id);

            _dbContext.Contracts.Remove(contractToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}