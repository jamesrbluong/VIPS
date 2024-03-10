using Common.Data;
using Common.Entities;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Repositories.Contracts;
using Repositories.CSVs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CSVs
{
    public class CSVService : ICSVService
    {
        private readonly ICSVRepository _csvRepository;

        public CSVService(ApplicationDbContext db, ICSVRepository csvRepository)
        {
            _csvRepository = csvRepository;
        }

        public async Task<Common.Entities.CSV> GetById(int contractId, CancellationToken ct)
        {
            return await _csvRepository.GetByIdAsync(contractId, ct);
        }

        public async Task<List<Common.Entities.CSV>> GetCSVsAsync(CancellationToken ct)
        {
            return await _csvRepository.GetListAsync(ct);
        }

        public void UploadCSVFile(IFormFile file)
        {
            var records = new List<CSV>();

            if (file != null)
            {
                using (var streamReader = new StreamReader(file.OpenReadStream()))
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    streamReader.ReadLine();
                    streamReader.ReadLine();
                    records = csvReader.GetRecords<CSV>().ToList();
                }

                _csvRepository.AddRange(records);


            }
        }
    }
}
