﻿using CsvHelper;
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
            TransferData();
            DeleteAllDataFromTable();
            return RedirectToAction("Index");
        }

        public IActionResult Submit()
        {
            TransferData();
            DeleteAllDataFromTable();
            return RedirectToAction("Index");
        }


        public void DeleteAllDataFromTable()
        {
            // Retrieve all records from the table
            var data = _db.CSVs.ToList();

            // Remove each record from the DbSet
            _db.CSVs.RemoveRange(data);

            // Save changes to the database
            _db.SaveChanges();
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
                    ContractID = csvItem.ContractID, // Map CSV properties to Contract properties
                    /*ContractID = csvItem.CSVProperty2,*/
                    // Map other properties as needed
                };

                contractData.Add(contractItem);
            }
            // Remove each record from the DbSet
            _db.CSVs.RemoveRange(csvData);
            _db.SaveChanges();
        }
    }

    }
