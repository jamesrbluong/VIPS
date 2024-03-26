using Common.Entities;

namespace Repositories.Partners
{
    public interface IPartnerRepository
    {
        Task AddAsync(Partner partner, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Partner> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<Partner>> GetListAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Partner partner, CancellationToken cancellationToken);
    }
}