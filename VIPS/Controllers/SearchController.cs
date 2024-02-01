using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VIPS.Common.Data;
using VIPS.Models.ViewModels.Search;
using VIPS.Repositories.Contracts;

namespace VIPS.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IContractRepository _contractsRepository;

        public SearchController(ApplicationDbContext db, IContractRepository contractsRepository)
        {
            _db = db;
            _contractsRepository = contractsRepository;
        }

        public async Task<IActionResult> SearchView(string sortOrder, string searchString, CancellationToken cancellationToken)
        {
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "alphabetical" : "";
            ViewData["DateSortParm"] = sortOrder == "close_exp" ? "far_exp" : "close_exp";
            ViewData["CurrentFilter"] = searchString;

            // Note: If there were a lot of actions being performed then we could have a service
            // which consumes the repo and then spits out the below result.
            var tempContracts = await _contractsRepository.GetListAsync(cancellationToken);
            var contractList = tempContracts.Select(x => CondensedContract.CreateFromContract(x));

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


            if (!string.IsNullOrEmpty(searchString))
            {
                model.ContractList = model.ContractList
                    .Where(c => c.ContractName.Contains(searchString))
                    .ToList();
            }

            // Note: Materialization can be a fun topic. 
            // Filter, search, etc. first then peform mat with something like ToListAsync. Saves on 
            // cost and time.
            // Design patterns. Gang of Four.
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

        public async Task<IActionResult> Contract (int id)
        {
            var contract = await _db.Contracts.Where(x => x.ContractID == id).FirstOrDefaultAsync();

            return View(contract);
        }
    }
}
