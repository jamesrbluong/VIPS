using CsvHelper;
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
using Repositories.Contracts;
using Services.Contracts;
using Repositories.CSVs;
using Services.CSVs;

namespace VIPS.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class CSVController : Controller
    {

        // "the goal is for the controller to just communicate with service. no db or repository -joshua" - matthew
        private readonly ApplicationDbContext _db;
        private readonly ICSVRepository _csvRepository;
        private readonly ICSVService _csvService;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken ct;

        public CSVController(ApplicationDbContext db, ICSVRepository csvRepository, ICSVService csvService)
        {
            _db = db;
            _csvRepository = csvRepository;
            _csvService = csvService;
            ct = _cancellationTokenSource.Token;
        }

        public async Task<IActionResult> Upload()
        {

            var data = await _csvService.GetCSVsAsync(ct);
            ViewBag.Count = data.Count;

            int countOfDuplicates = data.Count(model => model.Duplicate);
            ViewBag.Duplicate = data.Count - countOfDuplicates;

            return View(data);
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            _csvService.UploadCSVFile(file);
            _db.SaveChanges();

            CheckForDuplicates();

            ErrorChecking();
            _db.SaveChanges();

            return RedirectToAction("Upload", "CSV");
        }


        public async Task<IActionResult> ErrorExportCSVAsync()
        {

            byte[] fileBytes = await _csvService.ErrorExportCSVAsync(ct);

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
                return RedirectToAction(nameof(Index));
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

        public IActionResult OverWriteSubmit()
        {
            DeleteEdgeDataFromTable(); // here
            DeleteContractDataFromTable();
            DeleteSchoolDataFromTable(); // here
            PopulateSchools();
            DeletePartnerDataFromTable();
            PopulatePartners();
            DeleteDepartmentDataFromTable(); // here
            PopulateDepartments();

            SetNodeDataJavascipt();

            TransferData();
            DeleteCSVDataFromTable();

            return RedirectToAction("Upload");
        }

        public IActionResult Submit()
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

            DeleteDatabaseEntries();
            PopulateDatabaseEntries();

            TransferData();
            DeleteCSVDataFromTable();
            return RedirectToAction("Upload");
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


        public void TransferData()
        {
            // Retrieve all records from the table
            var csvData = _db.CSVs.ToList();
            // var contractData = _db.Contracts.ToList();

            foreach (var csvItem in csvData)
            {
                var contractItem = new Common.Entities.Contract
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

                // Console.WriteLine("pasta" + contractItem.COEHSPrograms);

                string from = "N/A";
                string to = "N/A";

                if (!string.IsNullOrEmpty(contractItem.Department))
                {
                    from = contractItem.Department;
                    to = contractItem.AgencyName;
                    AddVisualizationConnection(contractItem.ContractID, from, to, false);
                }
                else if (!string.IsNullOrEmpty(contractItem.COEHSPrograms))
                {
                    from = contractItem.COEHSPrograms;
                    to = contractItem.AgencyName;
                    AddVisualizationConnection(contractItem.ContractID, from, to, false);
                }
                else if (!string.IsNullOrEmpty(FolderNameRegex(contractItem.FolderName))) // FIX dept/coehs may be blank thats not good
                {
                    Console.WriteLine("isSchool test" + contractItem.Department + " " + contractItem.COEHSPrograms);
                    from = FolderNameRegex(contractItem.FolderName);
                    to = contractItem.AgencyName;

                    AddVisualizationConnection(contractItem.ContractID, from, to, true);
                }
            }
            // Remove each record from the DbSet
            _db.CSVs.RemoveRange(csvData);
            AddSchoolToDepartmentConnections();

            _db.SaveChanges();
        }

        public void DeleteDatabaseEntries()
        {
            DeleteEdgeDataFromTable(); // here
            DeleteContractDataFromTable();
            DeleteDepartmentDataFromTable(); // here
            DeletePartnerDataFromTable();
        }

        public void PopulateDatabaseEntries()
        {
            PopulateSchools();
            PopulateDepartments();
            PopulatePartners();
        }

        public string FolderNameRegex(string FolderName)
        {
            var split = FolderName.Split('\\');
            if (split.Length >= 2)
            {
                var result = split[split.Length - 2];
                result = Regex.Replace(result, @"\\.*$", "");
                return result;
            }

            return "";
        }

        public void PopulateSchools()
        {
            List<string> schoolNames = new List<string>();
            var folderName = _db.CSVs
                .Where(csv => !string.IsNullOrEmpty(csv.FolderName))
                .Select(csv => csv.FolderName.Trim())
                .Distinct()
                .ToList();

            foreach (var folder in folderName)
            {
                var schoolName = FolderNameRegex(folder);

                Console.WriteLine("FolderName test 2" + schoolName);

                schoolNames.Add(schoolName);
            }

            foreach (var name in schoolNames)
            {
                var school = new School
                {
                    Name = name
                };
                _db.Schools.Add(school);
            }

            _db.SaveChanges();
        }
        public void PopulateDepartments() // needs to also get school names from contract and then grab id from them
        {
            var deptData = _db.CSVs
                .Where(csv => !string.IsNullOrEmpty(csv.Department) && !string.IsNullOrEmpty(csv.FolderName)) // !string.IsNullOrEmpty(csv.Department) && 
                .Select(csv => new { dept = csv.Department, folderName = csv.FolderName })
                .Distinct()
                .ToList();

            var COEHSData = _db.CSVs
                .Where(csv => string.IsNullOrEmpty(csv.Department) && !string.IsNullOrEmpty(csv.FolderName) && !string.IsNullOrEmpty(csv.COEHSPrograms))
                .Select(csv => new { dept = csv.COEHSPrograms, folderName = csv.FolderName })
                .Distinct()
                .ToList();

            COEHSData.ForEach(Console.WriteLine);

            deptData = deptData.Concat(COEHSData).ToList();

            foreach (var item in deptData)
            {
                // Console.WriteLine("howdy "+ item.dept);
                var schoolId = _db.Schools
                    .Where(school => school.Name.Equals(FolderNameRegex(item.folderName)))
                    .Select(school => school.SchoolId)
                    .FirstOrDefault();

                var dept = new Department
                {
                    Name = item.dept,
                    SchoolId = schoolId
                };
                _db.Departments.Add(dept);

            }
            _db.SaveChanges();
        }

        public void PopulatePartners()
        {
            var partnerData = _db.CSVs
                .Where(csv => !string.IsNullOrEmpty(csv.AgencyName))
                .Select(csv => csv.AgencyName.Trim())
                .Distinct()
                .ToList();

            foreach (var partnerItem in partnerData)
            {
                var partner = new Partner
                {
                    Name = partnerItem
                };
                _db.Partners.Add(partner);
            }
            _db.SaveChanges();

        }
        public void AddVisualizationConnection(int ContractId, string FromName, string ToName, bool isSchool)
        {
            string FromId = "";
            string ToId = "";

            if (isSchool)
            {
                FromId = "s" + _db.Schools
                .Where(school => FromName.Equals(school.Name))
                .Select(school => school.SchoolId)
                .FirstOrDefault();
                ToId = "p" + _db.Partners
                .Where(partner => ToName.Equals(partner.Name))
                .Select(partner => partner.PartnerId)
                .FirstOrDefault();
            }
            else
            {
                FromId = "d" + _db.Departments
                .Where(dept => FromName.Equals(dept.Name))
                .Select(dept => dept.DepartmentId)
                .FirstOrDefault();

                ToId = "p" + _db.Partners
                .Where(partner => ToName.Equals(partner.Name))
                .Select(partner => partner.PartnerId)
                .FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(FromId) && !string.IsNullOrEmpty(ToId))
            {
                var connection = new Edge
                {
                    ContractId = ContractId,
                    FromId = FromId,
                    ToId = ToId
                };
                _db.Edges.Add(connection);
                _db.SaveChanges();
            }
        }

        public void AddSchoolToDepartmentConnections()
        {
            string FromId = "N/A";
            string ToId = "N/A";

            var departments = _db.Departments.ToList();
            Console.WriteLine("test 1");
            foreach (var department in departments)
            {
                FromId = "s" + department.SchoolId;
                ToId = "d" + department.DepartmentId;

                var connection = new Edge
                {
                    ContractId = 0,
                    FromId = FromId,
                    ToId = ToId
                };
                Console.WriteLine("test 2" + connection.FromId + " and " + connection.ToId);

                _db.Edges.Add(connection);
            }

            _db.SaveChanges();
        }

        public void SetNodeDataJavascipt()
        {
            var filePath = @"wwwroot\js\loadVisualization.js";
            string script = System.IO.File.ReadAllText(filePath);

            TempData["loadVisualization"] = script;
        }

        public void DeleteEdgeDataFromTable()
        {
            var data = _db.Edges.ToList();
            _db.Edges.RemoveRange(data);
            _db.SaveChanges();
        }

        public void DeleteDepartmentDataFromTable()
        {
            var data = _db.Departments.ToList();
            _db.Departments.RemoveRange(data);
            _db.SaveChanges();
        }


        public void DeletePartnerDataFromTable()
        {
            var data = _db.Partners.ToList();
            _db.Partners.RemoveRange(data);
            _db.SaveChanges();
        }

        public void DeleteSchoolDataFromTable()
        {
            var data = _db.Schools.ToList();
            _db.Schools.RemoveRange(data);
            _db.SaveChanges();
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

