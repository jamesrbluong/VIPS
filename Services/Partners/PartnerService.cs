using Common.Data;
using Common.Entities;
using Repositories.Partners;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Partners
{
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;

        public PartnerService(ApplicationDbContext db, IPartnerRepository partnerRepository)
        {
            _partnerRepository = partnerRepository;
        }

        public async Task<Common.Entities.Partner> GetById(int partnerId, CancellationToken ct)
        {
            return await _partnerRepository.GetByIdAsync(partnerId, ct);
        }

        public async Task<List<Common.Entities.Partner>> GetPartnersAsync (CancellationToken ct)
        {
            return await _partnerRepository.GetListAsync(ct);
        }

        
    }
}
