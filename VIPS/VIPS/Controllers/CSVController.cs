using CsvHelper;
using CSVParser.Data;
using CSVParser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;

namespace CSVParser.Controllers
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
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            var records = new List<CSV>();

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                records = csvReader.GetRecords<CSV>().ToList();
            }

            // Assuming validation and any necessary transformation of records is done here

            // Save the records to the database
            _db.CSVs.AddRange(records);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
