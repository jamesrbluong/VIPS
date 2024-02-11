using Common.Entities;

namespace Services.Visualizations
{
    public interface IVisualizationService
    {
        Task<List<Contract>> GetContractsAsync(CancellationToken ct);
        Task<List<Department>> GetDepartmentsAsync(CancellationToken ct);
        Task<List<Partner>> GetPartnersAsync(CancellationToken ct);
        Task<List<School>> GetSchoolsAsync(CancellationToken ct);
        Task<Common.Entities.Contract> FillContractDataAsync(int id, CancellationToken ct);
        Task<object> FillSchoolDataAsync(string stringId, CancellationToken ct);
        Task<object> FillDepartmentData(string departmentId, CancellationToken ct);
        Task<object> FillPartnerData(string partnerId, CancellationToken ct);
        Task<List<Edge>> GetNodesAsync(CancellationToken ct);
        Task<List<Edge>> GetEdgesAsync(CancellationToken ct);
    }
}