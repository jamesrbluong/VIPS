﻿using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using System.Diagnostics.Contracts;
using Common.Data;
using Common.Entities;
using Repositories.CSV;
using Services.CSV;

namespace VIPS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CSVController : Controller
    {

        // "the goal is for the controller to just communicate with service. no db or repository -joshua" - matthew
        private readonly ApplicationDbContext _db;

        private readonly ICSVService _CSVService;
        private readonly ICSVRepository _CSVRepository;

        public CSVController(ApplicationDbContext db, ICSVRepository CSVRepository, ICSVService CSVService)
        {
            _db = db;
            _CSVRepository = CSVRepository;
            _CSVService = CSVService;
        }

        public async Task<IActionResult> UploadAsync(CancellationToken ct)
        {
            var data = await _CSVService.GetCSVsAsync(ct);
            ViewBag.Count = data.Count;

            int countOfDuplicates = data.Count(model => model.Duplicate);
            ViewBag.Duplicate = data.Count - countOfDuplicates;

            return View(data);
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            _CSVService.UploadCSVFile(file);
            _db.SaveChanges();

            CheckForDuplicates();

            ErrorChecking();
            _db.SaveChanges();

            return RedirectToAction("Upload", "CSV");
        }


        public async Task<IActionResult> ErrorExportCSVAsync(CancellationToken ct)
        {

            byte[] fileBytes = await _CSVService.ErrorExportCSVAsync(ct);

            // Set the file name
            string fileName = "CSV_Error_Export.csv";

            // Return the CSV file
            return File(fileBytes, "text/csv", fileName);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.CSVs == null)
            {
                return NotFound();
            }

            var cSV = await _db.CSVs.FindAsync(id);
            if (cSV == null)
            {
                return NotFound();
            }
            return View(cSV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContractID,CreatedOn,CreatedBy,ContractName,ContractOrigin,ContractTypeName,CurrentStageAssignees,DaysInCurrStage,Description,ExternalContractReferenceID,FolderName,Locked,Owner,PrimaryDocument,RelatedToContract,RelatedToContractID,StageName,UpdatedBy,UpdatedOn,Workflow,ProgramsOrCourses,CCECMajors,AutoRenewal,ContractCategory,AgencyMailingAddress1,AgencyMailingAddress2,AgencyName,BCH_AgingServicesManagement,BCH_AthleticTraining,BCH_College,BCH_ExerciseScience,BCH_HealthAdministration,BCH_InterdisciplinaryHealthStudies,BCH_MentalHealthCounseling,BCH_NurseAnesthetist,BCH_Nursing,BCH_NutritionDietetics,BCH_PhysicalTherapy,BCH_PublicHealth,City,COEHSPrograms,Department,EmailAddress,FacultyInitiator,Graduate_Undergraduate,PhoneNumber,PrimaryContact,Renewal,State,TitleCert,Year,ZipCode,Error,ErrorDescription,Duplicate")] CSV cSV)
        {
            if (id != cSV.ContractID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(cSV);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CSVExists(cSV.ContractID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Upload");
            }
            return View(cSV);
        }

        private bool CSVExists(int id)
        {
            return (_db.CSVs?.Any(e => e.ContractID == id)).GetValueOrDefault();
        }

        private void ErrorChecking()
        {
            //Go through every variable in the contract and check to make sure they are useable 
            ErrorCheckingDepartments();
            ErrorCheckingContractID();
            ErrorCheckingZipCode();
            ErrorCheckingContractOrigin();
        }

        private void ErrorCheckingDepartments()
        {
            //Go through every variable in the contract and check to make sure they are useable 
            var csvData = _db.CSVs.ToList();

            List<string> departmentData = new List<string>
            {
                "BCH_AgingServicesManagement",
                "BCH_AthleticTraining",
                "BCH_College",
                "BCH_ExerciseScience",
                "BCH_HealthAdministration",
                "BCH_InterdisciplinaryHealthStudies",
                "BCH_MentalHealthCounseling",
                "BCH_NurseAnesthetist",
                "BCH_Nursing",
                "BCH_NutritionDietetics",
                "BCH_PhysicalTherapy",
                "BCH_PublicHealth"
            };

            foreach (var csvItem in csvData)
            {
                foreach (var departmentItem in departmentData)
                {
                    // Use reflection to get the value of the property with the dynamic name
                    var property = csvItem.GetType().GetProperty(departmentItem);
                    if (property != null)
                    {
                        var departmentValue = (string)property.GetValue(csvItem);
                        if (departmentValue != "TRUE" && departmentValue != "FALSE")
                        {
                            csvItem.Error = true;
                            csvItem.ErrorDescription += $" {departmentItem} needs to be either TRUE or FALSE,";
                        }
                    }
                }

            }
        }

        private void ErrorCheckingContractID()
        {
            //Go through every variable in the contract and check to make sure they are useable 
            var csvData = _db.CSVs.ToList();

            foreach (var csvItem in csvData)
            {
                if (csvItem.ContractID.ToString().Length != 6)
                {

                    csvItem.Error = true;
                    csvItem.ErrorDescription += $" ContractID needs to be either TRUE or FALSE,";
                }
            }
        }

        private void ErrorCheckingZipCode()
        {
            //Go through every variable in the contract and check to make sure they are useable 
            var csvData = _db.CSVs.ToList();

            foreach (var csvItem in csvData)
            {
                if (csvItem.ZipCode.ToString().Length != 5)
                {

                    csvItem.Error = true;
                    csvItem.ErrorDescription += $" ZipCode needs to be 5 or more,";
                }
            }
        }

        private void ErrorCheckingContractOrigin()
        {
            // Go through every CSV item and check the ContractOrigin property
            var csvData = _db.CSVs.ToList();

            List<string> validContractOrigins = new List<string>
                    {
                "Bulk Loader",
                "User",
                "Copy"
                };

            foreach (var csvItem in csvData)
            {
                var contractOriginValue = csvItem.ContractOrigin;

                // Check if the contractOriginValue is not in the list of valid contract origins
                if (!validContractOrigins.Contains(contractOriginValue))
                {
                    csvItem.Error = true;
                    csvItem.ErrorDescription += $" Contract Origin must be one of: {string.Join(", ", validContractOrigins)},";
                }
            }
        }

        public async Task<IActionResult> OverWriteSubmitAsync(CancellationToken ct)
        {
            await _CSVService.DeleteDatabaseEntries(ct);
            await PopulateDatabaseEntries(ct);
            await _CSVService.TransferDataAsync(ct);

            return RedirectToAction("Upload");
        }

        public async Task<IActionResult> SubmitAsync(CancellationToken ct)
        {
            var csvData = _db.CSVs.ToList();
            foreach (var csvItem in csvData)
            {
                if (csvItem.Error)
                {
                    TempData["AlertMessage"] = "There is an error in one of your Contracts so you cannot submit. Please use Export Errors to get a full list.";
                    return RedirectToAction("Upload");
                }
            }

            await _CSVService.DeleteDatabaseEntries(ct);
            await PopulateDatabaseEntries(ct);
            await _CSVService.TransferDataAsync(ct);
            return RedirectToAction("Upload");
        }

        public async Task<IActionResult> ClearUpload(CancellationToken ct)
        {
            await _CSVService.DeleteCSVDataFromTable(ct);
            return RedirectToAction("Upload");
        }

        public void CheckForDuplicates()
        {
            if (_db.CSVs.ToList().Count > 0)
            {
                var csvData = _db.CSVs.ToList();
                var contractData = _db.Contracts.ToList();

                foreach (var csvItem in csvData)
                {
                    if (contractData.Any(contract => contract.ContractID == csvItem.ContractID))
                    {
                        // Update the variable in the matching CSV instance
                        csvItem.Duplicate = true;
                        // If you want to save changes to the database, you can do it here
                    }
                }
                _db.SaveChanges();
            }
        }

        public async Task PopulateDatabaseEntries(CancellationToken ct)
        {
            await _CSVService.PopulateSchoolsAsync(ct);
            await _CSVService.PopulateDepartmentsAsync(ct);
            await _CSVService.PopulatePartnersAsync(ct);
        }

        private static string RemovePunct(string input)
        {
            return Regex.Replace(input, @"[.,'\-]", "");
        }

        public async Task<IActionResult> DetailView(int id)
        {
            var contract = await _db.CSVs.FindAsync(id);

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

