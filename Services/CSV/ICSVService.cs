using Common.Entities;

namespace Services.CSV
{
    public interface ICSVService
    {
        Task AddEdgeAsync(int ContractId, string FromName, string ToName, DateTime? exp, bool isSchool, CancellationToken ct);
        Task AddSchoolToDepartmentConnectionsAsync(CancellationToken cancellationToken);
        Task DeleteCSVDataFromTable(CancellationToken ct);
        Task DeleteDatabaseEntries(CancellationToken ct);
        string GetSchoolName(string name, string program);
        int GetYearsUntilExpiration(Contract contract);
        Task PopulateDepartmentsAsync(CancellationToken ct);
        Task PopulateEdges(Contract contractItem, CancellationToken ct);
        Task PopulatePartnersAsync(CancellationToken ct);
        Task PopulateSchoolsAsync(CancellationToken ct);
        Task TransferDataAsync(CancellationToken ct);
    }
}