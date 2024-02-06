using Common.Entities;

namespace Services.Schools
{
    public interface ISchoolService
    {
        Task<School> GetById(int SchoolId);
        Task<List<School>> GetSchoolsAsync(CancellationToken ct);
    }
}