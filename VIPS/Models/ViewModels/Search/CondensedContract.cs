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
            // Find the index of "AA -"
            var index = contract.ContractName.IndexOf("AA -", StringComparison.OrdinalIgnoreCase);

            // If "AA -" is found and there are characters after it
            if (index >= 0 && contract.ContractName.Length > index + 5)
            {
                // Extract the substring after "AA -"
                var substring = contract.ContractName.Substring(index + 5).Trim();

                // Extract the first word after "AA -"
                var firstWord = substring.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

                // If a word is found, assign it to the ContractName
                if (!string.IsNullOrEmpty(firstWord))
                {
                    contract.ContractName = firstWord;
                }
            }

            return new CondensedContract()
            {
                AgencyName = contract.AgencyName,
                City = contract.City,
                ContractID = contract.ContractID,
                ContractName = contract.ContractName,
                CreatedOn = contract.CreatedOn,
                Department = contract.Department,
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