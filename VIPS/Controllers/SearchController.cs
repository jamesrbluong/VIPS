using Microsoft.AspNetCore.Mvc;
using VIPS.Models.ViewModels.Search;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                contractList = contractList
                    .Where(c => c.Department.ToLower().Equals(searchString.ToLower()) || c.ContractName.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }

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
            };

            // Sort contracts based on sortOrder
            switch (sortOrder)
            {
                case "id":
                    contractList = contractList.OrderBy(c => c.ContractID).ToList();
                    break;
                case "alphabetical":
                    contractList = contractList
                        .OrderBy(c => char.IsDigit(c.ContractName.FirstOrDefault())) // Sort by whether the first character is a digit
                        .ThenBy(c => c.ContractName, StringComparer.OrdinalIgnoreCase) // Then sort alphabetically ignoring case
                        .ToList();
                    break;

                case "close_exp":
                    contractList = contractList
                        .OrderBy(c => renewalPeriods.ContainsKey(c.Renewal) ? renewalPeriods[c.Renewal] : renewalPeriods["Unknown"])
                        .ThenBy(c => c.ContractName, StringComparer.OrdinalIgnoreCase)
                        .ToList(); // Sort closest to expiration
                    break;
                case "far_exp":
                    contractList = contractList
                        .OrderByDescending(c => renewalPeriods.ContainsKey(c.Renewal) ? renewalPeriods[c.Renewal] : renewalPeriods["Unknown"])
                        .ThenBy(c => c.ContractName, StringComparer.OrdinalIgnoreCase)
                        .ToList(); // Sort furthest from expiration
                    break;
                default:
                    contractList = contractList.OrderBy(c => c.ContractName).ToList(); // Default sorting
                    break;
            }

            // Create the view model
            var model = new SearchViewModel
            {
                ContractList = contractList.Select(x => CondensedContract.CreateFromContract(x)).ToList(),
                SearchQuery = searchString,
                TotalContracts = contractList.Count, // Assign the total number of contracts to the view model property
                SortOrder = sortOrder, // Set the sort order in the view model
                RenewalPeriods = renewalPeriods // Set the renewal periods in the view model
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
