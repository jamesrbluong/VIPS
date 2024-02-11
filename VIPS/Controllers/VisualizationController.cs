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

namespace VIPS.Controllers
{
    public class VisualizationController : Controller
    {
        private readonly IEdgeService _edgeService;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken ct;

        public VisualizationController(ApplicationDbContext db, IEdgeService edgeService)
        {
            _edgeService = edgeService;
            ct = _cancellationTokenSource.Token;
        }

        [HttpGet]
        public async Task<IActionResult> GetVisualizationDataAsync() // ?
        {
            var data = await _edgeService.GetVisualizationsAsync(ct);
            return Json(data);
        }

        public async Task<IActionResult> GetContractDataAsync()
        {
            var data = await _edgeService.GetContractsAsync(ct);
            return Json(data);
        }

        public async Task<IActionResult> GetSchoolDataAsync()
        {
            var data = await _edgeService.GetSchoolsAsync(ct); // if school has no depts, don't add
            // var data = _db.Schools.Where(x => x.Departments != null && x.Departments.Any());
            return Json(data);
        }

        public async Task<IActionResult> GetDepartmentDataAsync()
        {
            var data = await _edgeService.GetDepartmentsAsync(ct);
            return Json(data);
        }

        public async Task<IActionResult> GetPartnerDataAsync()
        {
            var data = await _edgeService.GetPartnersAsync(ct);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> FillContractData(int contractId)
        {
            if (contractId != 0)
            {
                var contract = await _edgeService.FillContractDataAsync(contractId, ct);
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
                var data = await _edgeService.FillSchoolDataAsync(stringId, ct);
                return Json(data);
            }

            return default;
        }
        

        [HttpGet]
        public async Task<JsonResult> FillDepartmentDataAsync(string departmentId)
        {
            if (!string.IsNullOrEmpty(departmentId))
            {
                var data = await _edgeService.FillDepartmentData(departmentId, ct);
                return Json(data);
            }

            return default;
        }

        [HttpGet]
        public async Task<IActionResult> FillPartnerDataAsync(string partnerId)
        {
            if (!string.IsNullOrEmpty(partnerId))
            {
                var data = await _edgeService.FillPartnerData(partnerId, ct);
                return Json(data);
            }

            return default;
        }

        [HttpPost]
        public async Task<IActionResult> SetNodes (Node node)
        {
            if (node != null)
            {

            }

            return default;
        }
        


    }
}

