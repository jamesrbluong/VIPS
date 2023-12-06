using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VIPS.Models;
using VIPS.Models.ViewModels.Search;

namespace VIPS.Controllers
{
    public class SearchController : Controller
    {

        private readonly ApplicationDbContext _db;

        public SearchController(ApplicationDbContext db)
        {
            _db = db;
        }



        public IActionResult SearchView()
        {
            var tempcontract = _db.Contracts.Select(x => new
            {
                x.ContractID,
                x.CreatedOn,
                x.ContractName,
                x.Owner,
                x.StageName,
                x.UpdatedOn,
                x.AgencyName,
                x.City,
                x.Department,
                x.FacultyInitiator,
                x.Renewal,
                x.State,
                x.Year
            });

            tempcontract.ToList();

            var model = new SearchViewModel()
            {
                ContractList = tempcontract
            };


            return View(model);

        }
    }
}