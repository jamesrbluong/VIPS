using Common.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Repositories.CSV
{
    public class CSVRepository : ICSVRepository
    {

        private readonly ApplicationDbContext _db;

        public CSVRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        public async Task DeleteDataFromTableAsync(string tableName, CancellationToken ct)
        {
            var dbSetProperty = _db.GetType().GetProperties().FirstOrDefault(p => p.Name == tableName);
            if (dbSetProperty != null)
            {
                var dbSet = dbSetProperty.GetValue(_db) as IQueryable<object>;
                if (dbSet != null)
                {
                    _db.RemoveRange(dbSet.Cast<object>().ToList());
                    await _db.SaveChangesAsync(ct);
                }

            }

        }

        public Task<int> DeleteDataFromTableAsyncCSV(CancellationToken ct)
        {
            return _db.Database.ExecuteSqlRawAsync("EXEC DeleteAllCSVData", ct);
        }

        public Task<List<Common.Entities.CSV>> GetListAsync(CancellationToken cancellationToken)
        {
            return _db.CSVs.ToListAsync(cancellationToken);
        }

        public async Task<Common.Entities.CSV> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.CSVs.FindAsync(id);
        }

        public async Task AddAsync(Common.Entities.CSV CSV, CancellationToken cancellationToken)
        {
            _db.CSVs.Add(CSV);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Common.Entities.CSV CSV, CancellationToken cancellationToken)
        {
            _db.CSVs.Update(CSV);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var CSVToDelete = await _db.CSVs.FindAsync(id);

            _db.CSVs.Remove(CSVToDelete);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public void AddRange(IEnumerable<Common.Entities.CSV> records)
        {
            _db.CSVs.AddRange(records);
        }

        public Task<int> TransferCSVToContractData(CancellationToken cancellationToken)
        {
            return _db.Database.ExecuteSqlRawAsync("EXEC CopyCSVDataToContracts", cancellationToken);
        }


    }
}
