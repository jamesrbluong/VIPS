using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CSVs
{
    public interface ICSVRepository
    {
        Task AddAsync(CSV csv, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<CSV> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<CSV>> GetListAsync(CancellationToken cancellationToken);
        Task UpdateAsync(CSV csv, CancellationToken cancellationToken);
    }
}
