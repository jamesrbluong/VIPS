using CsvHelper;
using VIPS.Data;
using VIPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Text;
using Azure.Messaging;


namespace VIPS.Controllers
{
    public class CSVController : Controller
    {

        private readonly ApplicationDbContex _db;

        public CSVController(ApplicationDbContex db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            
            var data = _db.CSVs.ToList();
            ViewBag.Count = data.Count;

            int countOfDuplicates = data.Count(model => model.Duplicate);
            ViewBag.Duplicate = data.Count - countOfDuplicates;

            return View(data);
        }

        public IActionResult CSView()
        {
            var data = _db.CSVs.ToList();
            return View(data);
    }

        public IActionResult CSVDetail(int id)
        {
            var data = _db.CSVs.Where(x => x.ContractID == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            var records = new List<CSV>();

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                streamReader.ReadLine();
                streamReader.ReadLine();
                records = csvReader.GetRecords<CSV>().ToList();
            }

            
            // Save the records to the database
            _db.CSVs.AddRange(records);
            _db.SaveChanges();

            CheckForDuplicates();
            _db.SaveChanges();

            ErrorChecking();
            _db.SaveChanges();

            return RedirectToAction("Index");
        } 

/*        public IActionResult ToNotepad()
        {
            var csvData = _db.CSVs.ToList();
            string messageContent = "";

            foreach (var csvItem in csvData)
            {
                if (csvItem.Error)
                {
                    messageContent += csvItem.ContractID + " ";
                    messageContent += csvItem.ErrorDescription + " " + "\n";
                }
                
            }
            // Convert the string to bytes
            byte[] fileBytes = Encoding.UTF8.GetBytes(messageContent);

            // Set the file name
            string fileName = "model_info.txt";

            return File(fileBytes, "text/plain", fileName);
        }*/

        public IActionResult ToCSV()
        {
            var csvData = _db.CSVs.ToList();

            // Create a StringBuilder to build the CSV content
            var csvContent = new StringBuilder();

            // Add header row
            csvContent.AppendLine("ContractID,ErrorDescription");

            // Add data rows
            foreach (var csvItem in csvData)
            {
                if (csvItem.Error)
                {
                    csvContent.AppendLine($"{csvItem.ContractID},{csvItem.ErrorDescription}");
                }
            }

            // Convert the string to bytes
            byte[] fileBytes = Encoding.UTF8.GetBytes(csvContent.ToString());

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

        // POST: CSVs22/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContractID,CreatedOn,CreatedBy,ContractName,ContractOrigin,ContractTypeName,CurrentStageAssignees,DaysInCurrStage,Description,ExternalContractReferenceID,FolderName,Locked,Owner,PrimaryDocument,RelatedToContract,RelatedToContractID,StageName,UpdatedBy,UpdatedOn,Workflow,ProgramsOrCourses,CCECMajors,AutoRenewal,ContractCategory,AgencyMailingAddress1,AgencyMailingAddress2,AgencyName,BCH_AgingServicesManagement,BCH_AthleticTraining,BCH_College,BCH_ExerciseScience,BCH_HealthAdministration,BCH_InterdisciplinaryHealthStudies,BCH_MentalHealthCounseling,BCH_NurseAnesthetist,BCH_Nursing,BCH_NutritionDietetics,BCH_PhysicalTherapy,BCH_PublicHealth,City,COEHSPrograms,Department,EmailAddress,FacultyInitiator,Graduate_Undergraduate,PhoneNumber,PrimaryContact,Renewal,State,TitleCert,Year,ZipCode,Error,ErrorDescription,Duplicate")] CSV cSV)
        {
            if (id != cSV.Id)
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
                    if (!CSVExists(cSV.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cSV);
        }

        private bool CSVExists(int id)
        {
            return (_db.CSVs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void ErrorChecking()
        {
            //Go through every variable in the contract and check to make sure they are useable 
            ErrorCheckingDepartments();
            ErrorCheckingContractID();
            ErrorCheckingZipCode();
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

        _db.SaveChanges();
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
            _db.SaveChanges();
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
            _db.SaveChanges();
        }

        public IActionResult OverWriteSubmit()
        {
            DeleteContractDataFromTable();
            TransferData();
            DeleteCSVDataFromTable();
            return RedirectToAction("Index");
        }

        public IActionResult Submit()
        {
            var csvData = _db.CSVs.ToList();
            foreach (var csvItem in csvData)
            {
                if (csvItem.Error)
                {
                    TempData["AlertMessage"] = "There is an error in one of your Contracts so you cannot submit. Please use Export Errors to get a full list.";
                    return RedirectToAction("Index");
                }
            }
            DeleteContractDataFromTable();
            TransferData();
            DeleteCSVDataFromTable();
            return RedirectToAction("Index");
        }

        public void DeleteContractDataFromTable()
        {
            var data = _db.Contracts.ToList();
            _db.Contracts.RemoveRange(data);
            _db.SaveChanges();
        }

        public void DeleteCSVDataFromTable()
        {
            var data = _db.CSVs.ToList();
            _db.CSVs.RemoveRange(data);
            _db.SaveChanges();
        }

        public IActionResult ClearUpload()
        {
            var data = _db.CSVs.ToList();
            _db.CSVs.RemoveRange(data);
            _db.SaveChanges();
            return RedirectToAction("Index");
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


        public void TransferData()
        {
            // Retrieve all records from the table
            var csvData = _db.CSVs.ToList();
            var contractData = _db.Contracts.ToList();

            foreach (var csvItem in csvData)
            {
                var contractItem = new Contract
                {
                    ContractID = csvItem.ContractID,
                    RelatedToContractID = csvItem.RelatedToContractID,
                    CreatedOn = csvItem.CreatedOn,
                    CreatedBy = csvItem.CreatedBy,
                    ContractName = csvItem.ContractName,
                    ContractOrigin = csvItem.ContractOrigin,
                    ContractTypeName = csvItem.ContractTypeName,
                    CurrentStageAssignees = csvItem.CurrentStageAssignees,
                    DaysInCurrStage = csvItem.DaysInCurrStage,
                    Description = csvItem.Description,
                    ExternalContractReferenceID = csvItem.ExternalContractReferenceID,
                    FolderName = csvItem.FolderName,
                    Locked = csvItem.Locked,
                    Owner = csvItem.Owner,
                    PrimaryDocument = csvItem.PrimaryDocument,
                    RelatedToContract = csvItem.RelatedToContract,
                    StageName = csvItem.StageName,
                    UpdatedBy = csvItem.UpdatedBy,
                    UpdatedOn = csvItem.UpdatedOn,
                    Workflow = csvItem.Workflow,
                    ProgramsOrCourses = csvItem.ProgramsOrCourses,
                    CCECMajors = csvItem.CCECMajors,
                    AutoRenewal = csvItem.AutoRenewal,
                    ContractCategory = csvItem.ContractCategory,
                    AgencyMailingAddress1 = csvItem.AgencyMailingAddress1,
                    AgencyMailingAddress2 = csvItem.AgencyMailingAddress2,
                    AgencyName = csvItem.AgencyName,
                    BCH_AgingServicesManagement = csvItem.BCH_AgingServicesManagement,
                    BCH_AthleticTraining = csvItem.BCH_AthleticTraining,
                    BCH_College = csvItem.BCH_College,
                    BCH_ExerciseScience = csvItem.BCH_ExerciseScience,
                    BCH_HealthAdministration = csvItem.BCH_HealthAdministration,
                    BCH_InterdisciplinaryHealthStudies = csvItem.BCH_InterdisciplinaryHealthStudies,
                    BCH_MentalHealthCounseling = csvItem.BCH_MentalHealthCounseling,
                    BCH_NurseAnesthetist = csvItem.BCH_NurseAnesthetist,
                    BCH_Nursing = csvItem.BCH_Nursing,
                    BCH_NutritionDietetics = csvItem.BCH_NutritionDietetics,
                    BCH_PhysicalTherapy = csvItem.BCH_PhysicalTherapy,
                    BCH_PublicHealth = csvItem.BCH_PublicHealth,
                    City = csvItem.City,
                    COEHSPrograms = csvItem.COEHSPrograms,
                    Department = csvItem.Department,
                    EmailAddress = csvItem.EmailAddress,
                    FacultyInitiator = csvItem.FacultyInitiator,
                    Graduate_Undergraduate = csvItem.Graduate_Undergraduate,
                    PhoneNumber = csvItem.PhoneNumber,
                    PrimaryContact = csvItem.PrimaryContact,
                    Renewal = csvItem.Renewal,
                    State = csvItem.State,
                    TitleCert = csvItem.TitleCert,
                    Year = csvItem.Year,
                    ZipCode = csvItem.ZipCode
                };

                _db.Contracts.Add(contractItem);
            }
            // Remove each record from the DbSet
            _db.CSVs.RemoveRange(csvData);
            _db.SaveChanges();
        }

  
    }

    }
