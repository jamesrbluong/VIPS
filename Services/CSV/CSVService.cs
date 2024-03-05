﻿using Common.Entities;
using Humanizer;
using Repositories.Contracts;
using Repositories.CSV;
using Repositories.Departments;
using Repositories.Edges;
using Repositories.Nodes;
using Repositories.Partners;
using Repositories.Schools;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Services.CSV
{
    public class CSVService : ICSVService
    {
        private readonly IEdgeRepository _edgeRepository;
        private readonly IContractRepository _contractRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly ICSVRepository _CSVRepository;
        public CSVService(IContractRepository contractRepository, ISchoolRepository schoolRepository, IDepartmentRepository departmentRepository, IPartnerRepository partnerRepository, IEdgeRepository edgeRepository, ICSVRepository cSVRepository)
        {
            _contractRepository = contractRepository;
            _schoolRepository = schoolRepository;
            _departmentRepository = departmentRepository;
            _partnerRepository = partnerRepository;
            _edgeRepository = edgeRepository;
            _CSVRepository = cSVRepository;
        }

        public async Task PopulateEdges(Contract contractItem, CancellationToken ct)
        {
            string from = "N/A";
            string to = contractItem.AgencyName;
            DateTime? exp = null;
            bool isSchool = false;
            int years = GetYearsUntilExpiration(contractItem);

            if (years != -1)
            {
                exp = DateTime.Parse(contractItem.CreatedOn).AddYears(years);
            }

            var schoolName = GetSchoolName(contractItem.ContractTypeName, contractItem.ProgramsOrCourses);
            Console.WriteLine(schoolName + to + " PASTRAMI");


            if (!schoolName.Equals("Brooks College of Health"))
            {
                if (!string.IsNullOrEmpty(contractItem.Department))
                {
                    from = contractItem.Department;
                }
                else if (!string.IsNullOrEmpty(contractItem.COEHSPrograms))
                {
                    from = contractItem.COEHSPrograms;
                }
                else if (!string.IsNullOrEmpty(contractItem.CCECMajors))
                {
                    from = contractItem.CCECMajors;
                }
                else if (!string.IsNullOrEmpty(schoolName)) // FIX dept/coehs may be blank thats not good
                {
                    from = GetSchoolName(contractItem.ContractTypeName, schoolName);
                    isSchool = true;
                }
                else
                {
                    return;
                }
                await AddEdgeAsync(contractItem.ContractID, from, to, exp, isSchool, ct);
            }
            else
            {
                await BCHBruteForceAsync(contractItem, to, exp, isSchool, ct);
            }

        }

        public async Task BCHBruteForceAsync(Contract contractItem, string to, DateTime? exp, bool isSchool, CancellationToken ct)
        {
            Console.WriteLine("BCHBruteForce: " + contractItem.ContractName + ", " + to);
            List<string> BCHDepts = new List<string>
            {
                "BCH_College",
                "BCH_AgingServicesManagement",
                "BCH_AthleticTraining",
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

            if (contractItem.BCH_College.Equals("TRUE"))
            {
                Console.WriteLine("BCH_College is TRUE");
                string formattedName = "Brooks College of Health";
                isSchool = true;
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
                return;
            }

            if (contractItem.BCH_AgingServicesManagement.Equals("TRUE"))
            {
                string formattedName = FormatBCHName(BCHDepts[1]);
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
            }

            if (contractItem.BCH_AthleticTraining.Equals("TRUE"))
            {
                string formattedName = FormatBCHName(BCHDepts[2]);
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
            }

            if (contractItem.BCH_ExerciseScience.Equals("TRUE"))
            {
                string formattedName = FormatBCHName(BCHDepts[3]);
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
            }

            if (contractItem.BCH_HealthAdministration.Equals("TRUE"))
            {
                string formattedName = FormatBCHName(BCHDepts[4]);
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
            }

            if (contractItem.BCH_InterdisciplinaryHealthStudies.Equals("TRUE"))
            {
                string formattedName = FormatBCHName(BCHDepts[5]);
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
            }

            if (contractItem.BCH_MentalHealthCounseling.Equals("TRUE"))
            {
                string formattedName = FormatBCHName(BCHDepts[6]);
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
            }

            if (contractItem.BCH_NurseAnesthetist.Equals("TRUE"))
            {
                string formattedName = FormatBCHName(BCHDepts[7]);
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
            }

            if (contractItem.BCH_Nursing.Equals("TRUE"))
            {
                string formattedName = FormatBCHName(BCHDepts[8]);
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
            }

            if (contractItem.BCH_NutritionDietetics.Equals("TRUE"))
            {
                string formattedName = FormatBCHName(BCHDepts[9]);
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
            }

            if (contractItem.BCH_PhysicalTherapy.Equals("TRUE"))
            {
                string formattedName = FormatBCHName(BCHDepts[10]);
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
            }

            if (contractItem.BCH_PublicHealth.Equals("TRUE"))
            {
                string formattedName = FormatBCHName(BCHDepts[11]);
                await AddEdgeAsync(contractItem.ContractID, formattedName, to, exp, isSchool, ct);
            }
        }

        public async Task AddSchoolToDepartmentConnectionsAsync(CancellationToken cancellationToken)
        {
            string FromId = "N/A";
            string ToId = "N/A";

            var departments = await _departmentRepository.GetListAsync(cancellationToken);
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

                await _edgeRepository.AddAsync(connection, cancellationToken);
            }

        }

        public async Task AddEdgeAsync(int ContractId, string FromName, string ToName, DateTime? exp, bool isSchool, CancellationToken ct)
        {
            string FromId = "";
            string ToId = "";

            if (isSchool)
            {
                FromId = "s" + (await _schoolRepository.GetListAsync(ct))
                .Where(school => FromName.Equals(school.Name))
                .Select(school => school.SchoolId)
                .FirstOrDefault();
            }
            else
            {
                FromId = "d" + (await _departmentRepository.GetListAsync(ct))
                .Where(dept => FromName.Equals(dept.Name))
                .Select(dept => dept.DepartmentId)
                .FirstOrDefault();
            }

            ToId = "p" + (await _partnerRepository.GetListAsync(ct))
            .Where(partner => ToName.Equals(partner.Name))
            .Select(partner => partner.PartnerId)
            .FirstOrDefault();

            if (FromId.Equals("s0") || FromId.Equals("d0") || ToId.Equals("p0"))
            {
                Console.WriteLine(FromId + ", " + ToId);
                return;
            }
            else
            {
                var connection = new Edge
                {
                    ContractId = ContractId,
                    FromId = FromId,
                    ToId = ToId,
                    ExpirationDate = exp
                };
                await _edgeRepository.AddAsync(connection, ct);
            }
                
            
        }

        public async Task PopulatePartnersAsync(CancellationToken ct)
        {
            var partnerData = (await _CSVRepository.GetListAsync(ct))
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
                await _partnerRepository.AddAsync(partner, ct);
            }
        }

        public async Task PopulateDepartmentsAsync(CancellationToken ct) // needs to also get school names from contract and then grab id from them
        {
            var deptData = (await _CSVRepository.GetListAsync(ct))
                .Select(c => new {
                    school = c.ContractTypeName,
                    dept = !string.IsNullOrEmpty(c.Department) ?
                           c.Department :
                           !string.IsNullOrEmpty(c.COEHSPrograms) ?
                                c.COEHSPrograms :
                                (!string.IsNullOrEmpty(c.CCECMajors) ?
                                    c.CCECMajors : "")
                })
                .GroupBy(item => item.dept ) // Group by both school and dept
                .Select(group => group.First()) // Take the first item from each group
                .ToList();

            foreach (var item in deptData)
            {
                if (!string.IsNullOrEmpty(item.dept))
                {
                    var schoolName = GetSchoolName(item.school, "");

                    if (!schoolName.Equals("Brooks College of Health"))
                    {
                        var schoolId = (await _schoolRepository.GetListAsync(ct))
                        .Where(school => school.Name.Equals(schoolName))
                        .Select(school => school.SchoolId)
                        .FirstOrDefault();

                        var dept = new Department
                        {
                            Name = item.dept,
                            SchoolId = schoolId
                        };

                        await _departmentRepository.AddAsync(dept, ct);
                    }

                    
                }
            }

            List<string> BCHDepts = new List<string>
            {
                "BCH_AgingServicesManagement",
                "BCH_AthleticTraining",
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

            foreach (var item in BCHDepts)
            {
                var schoolId = (await _schoolRepository.GetListAsync(ct))
                    .Where(school => school.Name.Equals("Brooks College of Health"))
                    .Select(school => school.SchoolId)
                    .FirstOrDefault();

                string name = FormatBCHName(item);

                var dept = new Department
                {
                    Name = name,
                    SchoolId = schoolId
                };

                await _departmentRepository.AddAsync(dept, ct);
            }

        }

        public string FormatBCHName (string name)
        {
            name = name.Substring(name.IndexOf('_') + 1);
            name = Regex.Replace(name, @"(?<!^)([A-Z])", " $1");
            return name;
        }

        public async Task PopulateSchoolsAsync(CancellationToken ct)
        {
            List<string> schoolNames = new List<string>();

            var ContractTypeNames = (await _CSVRepository.GetListAsync(ct))
                .Where(csv => !string.IsNullOrEmpty(csv.ContractTypeName))
                .Select(csv => csv.ContractTypeName)
                .Distinct()
                .ToList();
            ContractTypeNames = ContractTypeNames.Select(n => Regex.Match(n, @"^[^-]*").Value.Trim()).Distinct().ToList();
            foreach (var n in ContractTypeNames)
            {
                var name = Regex.Match(n, @"^[^-]*").Value.Trim();

                var schoolName = GetSchoolName(name, "");

                Console.WriteLine("FolderName test 2" + schoolName + name);
                if (!string.IsNullOrEmpty(schoolName))
                {
                    schoolNames.Add(schoolName);
                }
            }

            foreach (var name in schoolNames)
            {
                var school = new School
                {
                    Name = name
                };
                await _schoolRepository.AddAsync(school, ct);
            }

        }


        public string GetSchoolName(string name, string program)
        {
            name = Regex.Match(name, @"^[^-]*").Value.Trim();

            if (!string.IsNullOrEmpty(program) && (name.Equals("AA ADMIN")))
            {
                if (IsHonors(program))
                {
                    return "Hicks Honors College";
                }
                else
                {
                    return "Division of Academic & Student Affairs";
                }
            }

            Dictionary<string, string> SchoolNames = new Dictionary<string, string>
            {
                { "AA ADMIN", "Division of Academic & Student Affairs" },
                { "AA BCH", "Brooks College of Health" },
                { "AA CCEC", "College of Computing, Engineering, and Construction" },
                { "AA COEHS", "College of Education & Human Services" },
                { "AA CCOB", "Coggin College of Business" },
                { "AA COAS", "College of Arts & Sciences" },
                { "AA CCOB/SBDC", "Coggin College of Business" }
            };

            if (SchoolNames.ContainsKey(name))
            {
                return SchoolNames[name];
            }
            return "GetSchoolName - invalid";
            
        }

        public bool IsHonors(string program)
        {
            return program.ToLower().Contains("honor") ? true : false;
        }

        public int GetYearsUntilExpiration(Common.Entities.Contract contract)
        {
            Dictionary<string, int> numberWords = new Dictionary<string, int>()
                {
                    {"one", 1},
                    {"two", 2},
                    {"three", 3},
                    {"four", 4},
                    {"five", 5},
                    {"six", 6},
                    {"seven", 7},
                    {"eight", 8},
                    {"nine", 9},
                    {"ten", 10},
                };

            if (!contract.AutoRenewal.ToLower().Equals("true"))
            {
                foreach (var word in numberWords)
                {
                    if (contract.Renewal.ToLower().Contains(word.Key))
                    {
                        return word.Value;
                    }
                }
            }
            return -1;
        }

        public async Task TransferDataAsync(CancellationToken ct)
        {
            // Retrieve all records from the table
            var csvData = await _CSVRepository.GetListAsync(ct);
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

                await _contractRepository.AddAsync(contractItem, ct);

                // Josh's code was here! moved to service

                await PopulateEdges(contractItem, ct);

            }
            // Remove each record from the DbSet
            await AddSchoolToDepartmentConnectionsAsync(ct);
            await DeleteCSVDataFromTable(ct);
        }

        public async Task DeleteDatabaseEntries(CancellationToken ct)
        {
            await _CSVRepository.DeleteDataFromTableAsync("Edges", ct);
            await _CSVRepository.DeleteDataFromTableAsync("Contracts", ct);
            await _CSVRepository.DeleteDataFromTableAsync("Schools", ct);
            await _CSVRepository.DeleteDataFromTableAsync("Departments", ct);
            await _CSVRepository.DeleteDataFromTableAsync("Partners", ct);
        }

        public async Task DeleteCSVDataFromTable(CancellationToken ct)
        {
            await _CSVRepository.DeleteDataFromTableAsync("CSVs", ct);
        }



    }
}