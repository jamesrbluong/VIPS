using Common.Data;
using Repositories.Schools;


namespace Services.Schools
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;

        public SchoolService(ApplicationDbContext db, ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public async Task<Common.Entities.School> GetById(int SchoolId, CancellationToken ct)
        {
            return await _schoolRepository.GetByIdAsync(SchoolId, ct);
        }

        public async Task<List<Common.Entities.School>> GetSchoolsAsync(CancellationToken ct)
        {
            return await _schoolRepository.GetListAsync(ct);
        }


    }
}
