using VIPS.Models;
using Microsoft.AspNetCore.Mvc;
using VIPS.Models.ViewModels.Search;
using Common.Data;
using Repositories.Contracts;
using Services.Contracts;
using Repositories.Partners;
using Repositories.Schools;
using Services.Partners;
using Services.Schools;
using Services.Visualizations;
using Services.Departments;
using System.Threading;
using Common.Entities;
using Services.Edges;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace VIPS.Controllers
{
    public class VisualizationController : Controller
    {
        private readonly IVisualizationService _visualizationService;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken ct;

        public VisualizationController(ApplicationDbContext db, IVisualizationService visualizationService)
        {
            ct = _cancellationTokenSource.Token;
            _visualizationService = visualizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEdgeDataAsync() // ?
        {
            var data = await _visualizationService.GetEdgesAsync(ct);
            return Json(data);
        }
        public async Task<IActionResult> GetNodeDataAsync() // ?
        {
            var data = await _visualizationService.GetNodesAsync(ct);
            return Json(data);
        }

        public async Task<IActionResult> GetContractDataAsync()
        {
            var data = await _visualizationService.GetContractsAsync(ct);
            return Json(data);
        }

        public async Task<IActionResult> GetSchoolDataAsync()
        {
            var data = await _visualizationService.GetSchoolsAsync(ct); // if school has no depts, don't add
            // var data = _db.Schools.Where(x => x.Departments != null && x.Departments.Any());
            return Json(data);
        }

        public async Task<IActionResult> GetDepartmentDataAsync()
        {
            var data = await _visualizationService.GetDepartmentsAsync(ct);
            return Json(data);
        }

        public async Task<IActionResult> GetPartnerDataAsync()
        {
            var data = await _visualizationService.GetPartnersAsync(ct);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> FillContractData(int contractId)
        {
            if (contractId != 0)
            {
                var contract = await _visualizationService.FillContractDataAsync(contractId, ct);
                if (contract != null)
                {
                    var data = CondensedContract.CreateFromContract(contract);
                    return Json(data);
                }
            }

            return default;
        }

        [HttpGet]
        public async Task<JsonResult> FillSchoolDataAsync(string stringId)
        {
            if (!string.IsNullOrEmpty(stringId))
            {
                var data = await _visualizationService.FillSchoolDataAsync(stringId, ct);
                return Json(data);
            }

            return default;
        }
        

        [HttpGet]
        public async Task<JsonResult> FillDepartmentDataAsync(string departmentId)
        {
            if (!string.IsNullOrEmpty(departmentId))
            {
                var data = await _visualizationService.FillDepartmentData(departmentId, ct);
                return Json(data);
            }

            return default;
        }

        [HttpGet]
        public async Task<IActionResult> FillPartnerDataAsync(string partnerId)
        {
            if (!string.IsNullOrEmpty(partnerId))
            {
                var data = await _visualizationService.FillPartnerData(partnerId, ct);
                return Json(data);
            }

            return default;
        }

        [HttpPost]
        public async Task<JsonResult> SetNodes (string nodes)
        {
            List<Node> nodeList = JsonConvert.DeserializeObject<List<Node>>(nodes);

            if (!string.IsNullOrEmpty(nodes))
            {
                await _visualizationService.DeleteAllNodes(ct);
                
                foreach (var item in nodeList)
                {
                    Console.WriteLine(item.NodeId + " " + item.Name + " " + item.x + " " + item.y + " " + item.SchoolId);

                    Node node = new Node
                    {
                        NodeId = item.NodeId,
                        Name = item.Name,
                        x = item.x,
                        y = item.y,
                        SchoolId = item.SchoolId
                    };

                    await _visualizationService.AddNodeAsync(node, ct);

                }
                
                
            }


            return Json("success");
        }
        


    }
}

