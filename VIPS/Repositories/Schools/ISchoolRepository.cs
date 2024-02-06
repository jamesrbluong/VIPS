using Common.Entities;

namespace Repositories.Schools
{
    public interface ISchoolRepository
    {
        Task<List<School>> GetListAsync(CancellationToken cancellationToken);
        Task<Common.Entities.School> GetByIdAsync(int id);
        Task AddAsync(Common.Entities.School school, CancellationToken cancellationToken);
        Task UpdateAsync(Common.Entities.School school, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
}