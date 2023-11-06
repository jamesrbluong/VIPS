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
            return View();
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

            return RedirectToAction("Index");
        }

        public IActionResult ViewCSV()
        {
            var data = _db.CSVs.ToList(); // Replace YourModels with the name of your DbSet
            return View(data);
        }

        public IActionResult Submit()
        {
            //_db.SaveChanges();
            return View();
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
    }

    }
