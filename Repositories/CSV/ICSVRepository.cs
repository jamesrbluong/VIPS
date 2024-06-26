﻿
namespace Repositories.CSV
{
    public interface ICSVRepository
    {
        Task AddAsync(Common.Entities.CSV CSV, CancellationToken cancellationToken);
        void AddRange(IEnumerable<Common.Entities.CSV> records);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task DeleteDataFromTableAsync(string tableName, CancellationToken ct);
        Task<int> DeleteDataFromTableAsyncCSV(CancellationToken ct);
        Task<Common.Entities.CSV> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<Common.Entities.CSV>> GetListAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Common.Entities.CSV CSV, CancellationToken cancellationToken);
        Task<int> TransferCSVToContractData(CancellationToken cancellationToken);
    }
}