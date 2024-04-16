using Microsoft.EntityFrameworkCore;
using VIPS.Common.Data;
using VIPS.Common.Models.Entities;
using VIPS.Repositories.Contracts;

namespace VIPS.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ContractRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Contract>> GetListAsync(CancellationToken cancellationToken)
        {
            return _dbContext.Contracts.ToListAsync(cancellationToken);
        }
    }
}
