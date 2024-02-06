

using Repositories.Visualizations;
using Services.Contracts;
using Services.Departments;
using Services.Partners;
using Services.Schools;
using System.Threading;

namespace Services.Visualizations
{
    public class VisualizationService : IVisualizationService, IVisualizationService
    {
        private readonly IVisualizationRepository _visualizationRepository;


        private readonly IContractService _contractService;
        private readonly ISchoolService _schoolService;
        private readonly IDepartmentService _departmentService;
        private readonly IPartnerService _partnerService;
        public VisualizationService(IVisualizationRepository visualizationRepository, IContractService contractService, ISchoolService schoolService, IDepartmentService departmentService, IPartnerService partnerService)
        {
            _visualizationRepository = visualizationRepository;


            _contractService = contractService;
            _schoolService = schoolService;
            _departmentService = departmentService;
            _partnerService = partnerService;
        }

        // Visualization Repository Methods
        public async Task<List<Common.Entities.Visualization>> GetVisualizationsAsync(CancellationToken ct)
        {
            return await _visualizationRepository.GetListAsync(ct);
        }

        public async Task<Common.Entities.Contract> FillContractDataAsync(int id, CancellationToken ct)
        {
            // get the specific contract
            return await _contractService.GetById(id);
        }

        public async Task<object> FillSchoolDataAsync(string stringId, CancellationToken ct)
        {
            // remove the letter identifier from the front of id
            stringId = stringId.Remove(0, 1);
            int intId = Int32.Parse(stringId);

            return await _departmentService.GetBySchool(intId, ct); ;
        }


        // Other services methods
        public async Task<List<Common.Entities.Contract>> GetContractsAsync(CancellationToken ct)
        {
            var contracts = await _contractService.GetContractsAsync(ct);

            return contracts;
        }

        public async Task<List<Common.Entities.School>> GetSchoolsAsync(CancellationToken ct)
        {
            return await _schoolService.GetSchoolsAsync(ct);
        }

        public async Task<List<Common.Entities.Department>> GetDepartmentsAsync(CancellationToken ct)
        {
            return await _departmentService.GetDepartmentsAsync(ct);
        }
        public async Task<List<Common.Entities.Partner>> GetPartnersAsync(CancellationToken ct)
        {
            return await _partnerService.GetPartnersAsync(ct);
        }

    }
}
