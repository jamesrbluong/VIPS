using Microsoft.AspNetCore.Mvc;
using VIPS.Models.ViewModels.Search;
using Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions; // Add this using directive for Regex

namespace VIPS.Controllers
{
    public class SearchController : Controller
    {
        private readonly IContractService _contractService;

        public SearchController(IContractService contractService)
        {
            _contractService = contractService;
        }

        public async Task<IActionResult> SearchView(string searchString, string sortOrder, CancellationToken cancellationToken)
        {
            var contractList = await _contractService.GetContractsAsync(cancellationToken);

            // Apply search filter if searchString is not null or empty
            if (!string.IsNullOrEmpty(searchString))
            {
                contractList = contractList.Where(c => c.Department.ToLower().Equals(searchString.ToLower()) || c.ContractName.ToLower().Contains(searchString.ToLower())).ToList();
            }

            // Calculate the total number of contracts
            var totalContracts = contractList.Count;

            // Define a mapping of renewal periods to numerical values
            var renewalPeriods = new Dictionary<string, int>
            {
                { "Auto", 0 },
                { "One Year", 1 },
                { "Two Year", 2 },
                { "Three Year", 3 },
                { "Four Year", 4 },
                { "Five Year", 5 },
                { "Six Year", 6 },
                { "Seven Year", 7 },
                { "Eight Year", 8 },
                { "Nine Year", 9 },
                { "Ten Year", 10 },
                { "Unknown", int.MaxValue } // Default value for empty or null Renewal
                // Add more as needed
            };

            // Sort by renewal period
            contractList = sortOrder == "close_exp" ?
                contractList.OrderBy(c => renewalPeriods.ContainsKey(c.Renewal) ? renewalPeriods[c.Renewal] : renewalPeriods["Unknown"]).ToList() : // Sort closest to expiration
                contractList.OrderByDescending(c => renewalPeriods.ContainsKey(c.Renewal) ? renewalPeriods[c.Renewal] : renewalPeriods["Unknown"]).ToList(); // Sort furthest from expiration

            // Create the view model
            var model = new SearchViewModel
            {
                ContractList = contractList.Select(x => CondensedContract.CreateFromContract(x)).ToList(),
                SearchQuery = searchString,
                TotalContracts = totalContracts // Assign the total number of contracts to the view model property
            };

            return View(model);
        }

        public async Task<IActionResult> Contract(int id)
        {
            var contract = await _contractService.GetById(id, CancellationToken.None);

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