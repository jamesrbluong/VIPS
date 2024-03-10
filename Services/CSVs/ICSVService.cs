using Common.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CSVs
{
    public interface ICSVService
    {
        public Task<CSV> GetById(int contractId, CancellationToken ct);
        public Task<List<Common.Entities.CSV>> GetCSVsAsync(CancellationToken ct);
        public void UploadCSVFile(IFormFile file);
        Task<byte[]> ErrorExportCSVAsync(CancellationToken cancellationToken);
    }
}
