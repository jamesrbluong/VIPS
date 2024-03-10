using Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VIPS.Models.ViewModels.Search;
using Repositories.Contracts;
using Services.Contracts;
using VIPS.Models.ViewModels.Account;

namespace VIPS.Controllers
{
    public class SearchController : Controller
    {
        // the goal is for the controller to just communicate with service. no db or repository -joshua
        private readonly ApplicationDbContext _db;
        private readonly IContractRepository _contractRepository;
        private readonly IContractService _contractService;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken ct;

        public SearchController(ApplicationDbContext db, IContractRepository contractRepository, IContractService contractService)
        {
            _db = db;
            _contractRepository = contractRepository;
            _contractService = contractService;
            ct = _cancellationTokenSource.Token;

        }

        /**
        public List<Contract> SearchContractsByDepartment(string departmentName)
        {
            var filteredContracts = _db.Contracts
                .Where(contract => contract.FolderName.Contains(departmentName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return filteredContracts;
        }
        **/

        public async Task<IActionResult> SearchView(string searchString, string sortOrder, CancellationToken cancellationToken)
        {
            var tempContracts = await _contractService.GetContractsAsync(cancellationToken);
            var contractList = tempContracts.Select(x => CondensedContract.CreateFromContract(x)).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                contractList = contractList.Where(c => c.Department.ToLower().Equals(searchString.ToLower()) || c.ContractName.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (sortOrder == "alphabetical")
            {
                contractList = contractList.OrderBy(contract => {
                    var contractName = contract.ContractName;
                    var index = contractName.IndexOf("AA - ");
                    if (index >= 0 && contractName.Length > index + 5)
                    {
                        var substring = contractName.Substring(index + 5);
                        var words = substring.Split(' ');
                        if (words.Length > 0)
                        {
                            return words[0];
                        }
                    }
                    return contractName;
                }).ToList();
            }
            var model = new SearchViewModel
            {
                ContractList = contractList,
                SearchQuery = searchString
            };

            return View(model);
        }
        public async Task<IActionResult> Contract(int id)
        {
            var contract = await _contractService.GetById(id, ct);

            if (contract != null)
            {
                return View(contract);
            }
            else
            {
                Console.WriteLine("NotFound");
                return NotFound();
            }

        }
    }
}
