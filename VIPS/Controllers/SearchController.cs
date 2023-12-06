using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
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

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var contracts = from c in _db.Contracts
                            select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                contracts = contracts.Where(c => c.ContractName.Contains(searchString));
            }

            return View(await contracts.AsNoTracking().ToListAsync());
        }

        public IActionResult SearchView()
        {
            var tempcontracts = _db.Contracts.Select(x => new CondensedContract
            {
                ContractID = x.ContractID,
                CreatedOn = x.CreatedOn,
                ContractName = x.ContractName,
                Owner = x.Owner,
                StageName = x.StageName,
                UpdatedOn = x.UpdatedOn,
                AgencyName = x.AgencyName,
                City = x.City,
                Department = x.Department,
                FacultyInitiator = x.FacultyInitiator,
                Renewal = x.Renewal,
                State = x.State,
                Year =  x.Year
            }).ToList();

            var model = new SearchViewModel()
            {
                ContractList = tempcontracts
            };



            return View(model);
        }

    }


}



   