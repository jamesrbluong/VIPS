using Common.Entities;

namespace Services.Visualizations
{
    public interface IVisualizationService
    {
        Task<List<Contract>> GetContractsAsync(CancellationToken ct);
        Task<List<Department>> GetDepartmentsAsync(CancellationToken ct);
        Task<List<Partner>> GetPartnersAsync(CancellationToken ct);
        Task<List<School>> GetSchoolsAsync(CancellationToken ct);
        Task<List<Visualization>> GetVisualizationsAsync(CancellationToken ct);
        Task<Common.Entities.Contract> FillContractDataAsync(int id, CancellationToken ct);
        Task<object> FillSchoolDataAsync(string stringId, CancellationToken ct);

    }
}