using VIPS.Models;
using Microsoft.AspNetCore.Mvc;
using VIPS.Models.Data;


namespace VIPS.Controllers
{
    public class VisualizationController : Controller
    {

        private readonly ApplicationDbContext _db;
        public VisualizationController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult GetContractData()
        {
            var data = _db.Contracts.ToList();
            return Json(data);
        }

        public IActionResult GetPartnerData()
        {
            var data = _db.Partners.ToList();
            return Json(data);
        }

    }
}