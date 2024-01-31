using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VIPS.Models;
using VIPS.Models.Data;
using VIPS.Models.ViewModels.Search;
using System.Collections.Generic;

namespace VIPS.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SearchController(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Contract> SearchContractsByDepartment(string departmentName)
        {
            var filteredContracts = _db.Contracts
                .Where(contract => contract.FolderName.Contains(departmentName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return filteredContracts;
        }

        public async Task<IActionResult> SearchView(string searchString, string sortOrder)
        {
            var model = new SearchViewModel();

            if (!String.IsNullOrEmpty(searchString))
            {
                model.ContractList = await _db.Contracts
                    .Where(c => c.Department.ToLower() == searchString.ToLower() || c.ContractName.ToLower().Contains(searchString.ToLower()))
                    .Select(x => new CondensedContract
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
                        Year = x.Year,
                        
                      
                    }).ToListAsync();
            }
            else
            {
                model.ContractList = await _db.Contracts
                    .Select(x => new CondensedContract
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
                        Year = x.Year,
                        
                    }).ToListAsync();
            }

            if (sortOrder == "alphabetical")
            {
                model.ContractList = model.ContractList.OrderBy(contract => {
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
            /*else if (sortOrder == "close_exp")
            {
                model.ContractList = model.ContractList.OrderBy(contract => contract.ExpirationDate).ToList();
            }
            else if (sortOrder == "far_exp")
            {
                model.ContractList = model.ContractList.OrderByDescending(contract => contract.ExpirationDate).ToList();
            }
            // Add other sorting logics for 'id' if needed...
    */
            return View(model);
        }
        public IActionResult Contract(int id)
        {
            var contract = _db.Contracts.Where(x => x.ContractID == id).FirstOrDefault();

            return View(contract);
        }
    }
}
