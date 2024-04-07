using Common.Entities;

namespace VIPS.Models.ViewModels.Search
{
    public class CondensedContract
    {
        public int ContractID { get; set; }
        public string CreatedOn { get; set; }
        public string ContractName { get; set; }
        public string Owner { get; set; }
        public string StageName { get; set; }
        public string UpdatedOn { get; set; }
        public string AgencyName { get; set; }
        public string City { get; set; }
        public string Department { get; set; }
        public string FacultyInitiator { get; set; }
        public string Renewal { get; set; }
        public string State { get; set; }
        public string Year { get; set; }

        public static CondensedContract CreateFromContract(Contract contract)
        {
            return new CondensedContract()
            {
                AgencyName = contract.AgencyName,
                City = contract.City,
                ContractID = contract.ContractID,
                ContractName = contract.ContractName,
                CreatedOn = contract.CreatedOn,
                Department = !string.IsNullOrEmpty(contract.Department) ? contract.Department :
                             !string.IsNullOrEmpty(contract.COEHSPrograms) ? contract.COEHSPrograms :
                             !string.IsNullOrEmpty(contract.CCECMajors) ? contract.CCECMajors : "",
                FacultyInitiator = contract.FacultyInitiator,
                Owner = contract.Owner,
                Renewal = contract.Renewal,
                StageName = contract.StageName,
                State = contract.State,
                UpdatedOn = contract.UpdatedOn,
                Year = contract.Year
            };
        }



    }
}
