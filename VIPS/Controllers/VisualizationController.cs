using VIPS.Models;
using Microsoft.AspNetCore.Mvc;
using VIPS.Models.ViewModels.Search;
using Common.Data;
using Repositories.Contracts;
using Services.Contracts;
using Repositories.Partners;
using Repositories.Schools;
using Repositories.Visualizations;
using Services.Partners;
using Services.Schools;
using Services.Visualizations;
using Services.Departments;
using System.Threading;
using Common.Entities;

namespace VIPS.Controllers
{
    public class VisualizationController : Controller
    {
        private readonly IVisualizationService _visualizationService;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private CancellationToken ct;

        public VisualizationController(ApplicationDbContext db, IVisualizationService visualizationService)
        {
            _visualizationService = visualizationService;
            ct = _cancellationTokenSource.Token;
        }

        [HttpGet]
        public IActionResult GetVisualizationData()
        {
            var data = _visualizationService.GetVisualizationsAsync(ct);
            return Json(data);
        }

        public IActionResult GetContractData()
        {
            var data = _visualizationService.GetContractsAsync(ct);
            return Json(data);
        }

        public IActionResult GetSchoolData()
        {
            var data = _visualizationService.GetSchoolsAsync(ct); // if school has no depts, don't add
            // var data = _db.Schools.Where(x => x.Departments != null && x.Departments.Any());
            return Json(data);
        }

        public IActionResult GetDepartmentData()
        {
            var data = _visualizationService.GetDepartmentsAsync(ct);
            return Json(data);
        }

        public IActionResult GetPartnerData()
        {
            var data = _visualizationService.GetPartnersAsync(ct);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> FillContractDataAsync(int contractId)
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
        public JsonResult FillSchoolData(string stringId)
        {
            if (!string.IsNullOrEmpty(stringId))
            {
                var data = _visualizationService.FillSchoolDataAsync(stringId, ct);
                return Json(data);
            }

            return default;
        }
        

        [HttpGet]
        public JsonResult FillDepartmentData(string departmentId)
        {
            if (!string.IsNullOrEmpty(departmentId))
            {
                var data = _visualizationService.FillDepartmentData(departmentId, ct);
                return Json(data);
            }

            return default;
        }

        [HttpGet]
        public IActionResult FillPartnerData(string partnerId)
        {
            if (!string.IsNullOrEmpty(partnerId))
            {
                var data = _visualizationService.FillDepartmentData(partnerId, ct);
                return Json(data);
            }

            return default;
        }

        


    }
}