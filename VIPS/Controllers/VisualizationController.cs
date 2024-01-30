using VIPS.Models;
using Microsoft.AspNetCore.Mvc;
using VIPS.Models.Data;
using VIPS.Models.ViewModels.Search;

namespace VIPS.Controllers
{
    public class VisualizationController : Controller
    {

        private readonly ApplicationDbContext _db;
        public VisualizationController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult GetContractData()
        {
            var data = _db.Contracts.ToList();
            return Json(data);
        }

        public IActionResult GetPartnerData()
        {
            var data = _db.Partners.ToList();
            // data.ForEach(Console.WriteLine);
            return Json(data);
        }

        public IActionResult GetSchoolData()
        {
            var data = _db.Schools.ToList(); // if school has no depts, don't add
            // var data = _db.Schools.Where(x => x.Departments != null && x.Departments.Any());
            return Json(data);
        }

        public IActionResult GetDepartmentData()
        {
            var data = _db.Departments.ToList();
            return Json(data);
        }

        [HttpGet]
        public IActionResult GetVisualizationData()
        {
            var data = _db.Visualizations.ToList();
            return Json(data);
        }

        
        [HttpGet]
        public JsonResult FillSchoolData(string stringId)
        {
            // Console.WriteLine("test dept fill" + schoolId);

            // This is wrong?
            // Get a list of department names AND all contracts associated with those departments
            stringId = stringId.Remove(0,1);
            Console.WriteLine("noooo" + stringId);
            int intId = Int32.Parse(stringId);
            var deptNames = _db.Departments.Where(x => x.SchoolId == intId).Select(x => new { departmentName = x.Name }).ToList();
            
            var data = deptNames;

            // use this list of dept ids to further get the contracts using that id from visualization table (also connections from s to p)
            /*
            foreach (var deptId in deptData)
            {
                var connectionData = _db.Visualizations.Where(x => x.ToId == deptId).Select(x => new { contractId = x.ContractId }).ToList();
            }
            */
            

            return Json(data);
        }
        

        [HttpGet]
        public JsonResult FillDepartmentData(string departmentId)
        {
            Console.WriteLine("test dept fill" + departmentId);
            
            var data = _db.Visualizations.Where(x => x.FromId == departmentId).Select(x => new { contractId = x.ContractId } ).ToList();
            
            return Json(data);
        }

        [HttpGet]
        public IActionResult FillPartnerData(string partnerId)
        {
            var data = _db.Visualizations.Where(x => x.ToId == partnerId).Select(x => new { contractId = x.ContractId }).ToList();

            return Json(data);
        }

        [HttpGet]
        public IActionResult FillContractData(int contractId)
        {
            Console.WriteLine("test contract fill" + contractId);

            var contract = _db.Contracts.Where(x => x.ContractID == contractId).FirstOrDefault();
            if (contract != null)
            {
                var data = new CondensedContract
                {
                    ContractID = contractId,
                    CreatedOn = contract.CreatedOn,
                    ContractName = contract.ContractName,
                    Owner = contract.Owner,
                    StageName = contract.StageName,
                    UpdatedOn = contract.UpdatedOn,
                    AgencyName = contract.AgencyName,
                    City = contract.City,
                    Department = contract.Department,
                    FacultyInitiator = contract.FacultyInitiator,
                    Renewal = contract.Renewal,
                    State = contract.State,
                    Year = contract.Year
                };
                
                return Json(data);
            }
            return Json("");
            
        }


    }
}