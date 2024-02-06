using Common.Entities;

namespace Repositories.Schools
{
    public interface ISchoolRepository
    {
        Task AddAsync(School school, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<School> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<School>> GetListAsync(CancellationToken cancellationToken);
        Task UpdateAsync(School school, CancellationToken cancellationToken);
    }
}