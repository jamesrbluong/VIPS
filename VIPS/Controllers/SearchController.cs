using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<IActionResult> SearchView(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "alphabetical" : "";
            ViewData["DateSortParm"] = sortOrder == "close_exp" ? "far_exp" : "close_exp";
            ViewData["CurrentFilter"] = searchString;

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
                Year = x.Year
            }).ToList();

            var model = new SearchViewModel
            {
                ContractList = tempcontracts
            };


            if (!String.IsNullOrEmpty(searchString))
            {
                model.ContractList = model.ContractList
                    .Where(c => c.ContractName.Contains(searchString))
                    .ToList();
            }

            switch (sortOrder)
            {
                case "alphabetical":
                    model.ContractList = model.ContractList.OrderByDescending(c => c.ContractName).ToList();
                    break;
                case "id":
                    model.ContractList = model.ContractList.OrderBy(c => c.ContractID).ToList();
                    break;
                case "close_exp":
                    model.ContractList = model.ContractList.OrderBy(c => DateTime.Parse(c.CreatedOn)).ToList();
                    break;
                case "far_exp":
                    model.ContractList = model.ContractList.OrderByDescending(c => DateTime.Parse(c.CreatedOn)).ToList();
                    break;
                default:
                    model.ContractList = model.ContractList.OrderBy(c => c.ContractName).ToList();
                    break;
            }

            return View(model);

        }

        public IActionResult Contract (int id)
        {
            var contract = _db.Contracts.Where(x => x.ContractID == id).FirstOrDefault();

            return View(contract);
        }
    }
}
