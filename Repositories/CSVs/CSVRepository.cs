using Common.Data;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CSVs
{

    public class CSVRepository : ICSVRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CSVRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
