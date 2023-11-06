using CsvHelper;
using VIPS.Data;
using VIPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Text;

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

            // Assuming validation and any necessary transformation of records is done here

            // Save the records to the database
            _db.CSVs.AddRange(records);
            _db.SaveChanges();

            CheckForDuplicates();
            _db.SaveChanges();

            return RedirectToAction("Index");
        } 

        public IActionResult ToNotepad()
        {
            // Fetch data for the model (replace with your data retrieval logic)

            // Create a string with the model information
            string content = "Hello this is a test";

            // Convert the string to bytes
            byte[] fileBytes = Encoding.UTF8.GetBytes(content);

            // Set the file name
            string fileName = "model_info.txt";

            return File(fileBytes, "text/plain", fileName);
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
