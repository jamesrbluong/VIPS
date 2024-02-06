using Common.Entities;

namespace Services.Partners
{
    public interface IPartnerService
    {
        public Task<Partner> GetById(int contractId, CancellationToken ct);
        public Task<List<Common.Entities.Partner>> GetPartnersAsync(CancellationToken ct);
    }
}