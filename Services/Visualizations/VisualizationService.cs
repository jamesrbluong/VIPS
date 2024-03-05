using Common.Entities;
using Services.Contracts;
using Services.Departments;
using Services.Edges;
using Services.Nodes;
using Services.Partners;
using Services.Schools;
using System.Text.Json.Nodes;
using System.Threading;

namespace Services.Visualizations
{
    public class VisualizationService : IVisualizationService
    {
        private readonly INodeService _nodeService;
        private readonly IEdgeService _edgeService;
        private readonly IContractService _contractService;
        private readonly ISchoolService _schoolService;
        private readonly IDepartmentService _departmentService;
        private readonly IPartnerService _partnerService;
        public VisualizationService(IContractService contractService, ISchoolService schoolService, IDepartmentService departmentService, IPartnerService partnerService, INodeService nodeService, IEdgeService edgeService)
        {
            _contractService = contractService;
            _schoolService = schoolService;
            _departmentService = departmentService;
            _partnerService = partnerService;
            _nodeService = nodeService;
            _edgeService = edgeService;
        }



        public async Task<Common.Entities.Contract> FillContractDataAsync(int id, CancellationToken ct)
        {
            // get the specific contract
            return await _contractService.GetById(id, ct);
        }

        public async Task<object> FillSchoolDataAsync(string stringId, CancellationToken ct)
        {
            // remove the letter identifier from the front of id
            stringId = IsolateNodeId(stringId);
            int schoolId = Int32.Parse(stringId);

            var depts = await _departmentService.GetDepartmentsAsync(ct);
            var deptNames = depts.Where(x => x.SchoolId == schoolId).Select(x => new { departmentName = x.Name }).ToList();

            var connections = await GetEdgesAsync(ct);
            var contracts = connections.Where(x => x.FromId == "s" + schoolId && x.ContractId != 0).Select(x => new { contractId = x.ContractId }).ToList();

            var result = new
            {
                Depts = deptNames,
                Contracts = contracts
            };

            return result;
        }

        public async Task<object> FillDepartmentData(string departmentId, CancellationToken ct)
        {
            var connections = await GetEdgesAsync(ct);
            var contracts = connections.Where(x => x.FromId == departmentId).Select(x => new { contractId = x.ContractId }).ToList();

            return contracts;
        }

        public async Task<object> FillPartnerData(string partnerId, CancellationToken ct)
        {
            var connections = await GetEdgesAsync(ct);
            var contracts = connections.Where(x => x.ToId == partnerId).Select(x => new { contractId = x.ContractId }).ToList();

            return contracts;
        }

        public string IsolateContractId (string input)
        {
            Console.WriteLine("IsolateId: " + input);


            if (input.Length > 0 && char.IsLetter(input[0]))
            {
                input = input.Substring(1);
            }
            

            Console.WriteLine(input);

            return input;

        }

        public string IsolateNodeId (string input)
        {
            Console.WriteLine("IsolateNodeId: " + input);
            if (input.Length > 0 && char.IsLetter(input[0]))
            {
                input = input.Substring(1);
            }
            Console.WriteLine("IsolateNodeId: " + input);

            return input;
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

        public async Task<List<Common.Entities.Edge>> GetEdgesAsync(CancellationToken ct)
        {
            return await _edgeService.GetEdgesAsync(ct);
        }

        public async Task<List<Common.Entities.Node>> GetNodesAsync(CancellationToken ct)
        {
            return await _nodeService.GetNodesAsync(ct);
        }

        public async Task AddNodeAsync(Node node, CancellationToken ct)
        {
            await _nodeService.AddNodeAsync(node, ct);
        }
        public async Task DeleteAllNodes(CancellationToken cancellationToken)
        {
            await _nodeService.DeleteAllNodes(cancellationToken);
        }
    }
}

