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
            while (!cancellationToken.IsCancellationRequested)
            {
                return _dbContext.Contracts.ToListAsync(cancellationToken);
            }

            return default;
        }

        public async Task<Common.Entities.Contract> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return await _dbContext.Contracts.FindAsync(id);
            }

            return default;

        }

        public async Task AddAsync(Common.Entities.Contract contract, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Contracts.Add(contract);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task UpdateAsync(Common.Entities.Contract contract, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _dbContext.Contracts.Update(contract);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var contractToDelete = await _dbContext.Contracts.FindAsync(id);

                _dbContext.Contracts.Remove(contractToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return;
            }
        }
    }
}