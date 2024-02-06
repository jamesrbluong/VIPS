using Common.Entities;

namespace Repositories.Partners
{
    public interface IPartnerRepository
    {
        Task<List<Partner>> GetListAsync(CancellationToken cancellationToken);
        Task<Common.Entities.Partner> GetByIdAsync(int id);
        Task AddAsync(Common.Entities.Partner partner, CancellationToken cancellationToken);
        Task UpdateAsync(Common.Entities.Partner partner, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}