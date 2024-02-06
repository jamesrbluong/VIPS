using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Common.Entities
{
    public class CSV
    {
        [Key]
        [Ignore]
        public int Id { get; set; }

        [Name("Contract ID")]
        public int ContractID { get; set; }

        [Name("CreatedOn")]
        public string CreatedOn { get; set; }

        [Name("CreatedBy")]
        public string CreatedBy { get; set; }

        [Name("ContractName")]
        public string ContractName { get; set; }

        [Name("ContractOrigin")]
        public string ContractOrigin { get; set; }

        [Name("ContractTypeName")]
        public string ContractTypeName { get; set; }

        [Name("Current Stage Assignees")]
        public string CurrentStageAssignees { get; set; }

        [Name("DaysInCurrStage")]
        public string DaysInCurrStage { get; set; }

        [Name("Description")]
        public string Description { get; set; }

        [Name("ExternalContractReferenceID")]
        public string ExternalContractReferenceID { get; set; }

        [Name("FolderName")]
        public string FolderName { get; set; }

        [Name("Locked")]
        public string Locked { get; set; }

        [Name("Owner")]
        public string Owner { get; set; }

        [Name("PrimaryDocument")]
        public string PrimaryDocument { get; set; }

        [Name("RelatedToContract")]
        public string RelatedToContract { get; set; }

        [Name("RelatedToContractID")]
        public string RelatedToContractID { get; set; }

        [Name("StageName")]
        public string StageName { get; set; }

        [Name("UpdatedBy")]
        public string UpdatedBy { get; set; }

        [Name("UpdatedOn")]
        public string UpdatedOn { get; set; }

        [Name("Workflow")]
        public string Workflow { get; set; }

        [Name("Programs or Courses")]
        public string ProgramsOrCourses { get; set; }

        [Name("CCEC Majors")]
        public string CCECMajors { get; set; }

        [Name("Auto Renewal")]
        public string AutoRenewal { get; set; }

        [Name("Contract Category")]
        public string ContractCategory { get; set; }

        [Name("Agency Mailing Address 1")]
        public string AgencyMailingAddress1 { get; set; }

        [Name("Agency Mailing Address 2")]
        public string AgencyMailingAddress2 { get; set; }

        [Name("Agency Name")]
        public string AgencyName { get; set; }

        [Name("BCH - Aging Services Management")]
        public string BCH_AgingServicesManagement { get; set; }

        [Name("BCH - Athletic Training")]
        public string BCH_AthleticTraining { get; set; }

        [Name("BCH - College")]
        public string BCH_College { get; set; }

        [Name("BCH - Exercise Science")]
        public string BCH_ExerciseScience { get; set; }

        [Name("BCH - Health Administration")]
        public string BCH_HealthAdministration { get; set; }

        [Name("BCH - Interdisciplinary Health Studies")]
        public string BCH_InterdisciplinaryHealthStudies { get; set; }

        [Name("BCH - Mental Health Counseling")]
        public string BCH_MentalHealthCounseling { get; set; }

        [Name("BCH - Nurse Anesthetist")]
        public string BCH_NurseAnesthetist { get; set; }

        [Name("BCH - Nursing")]
        public string BCH_Nursing { get; set; }

        [Name("BCH - Nutrition & Dietetics")]
        public string BCH_NutritionDietetics { get; set; }

        [Name("BCH - Physical Therapy")]
        public string BCH_PhysicalTherapy { get; set; }

        [Name("BCH - Public Health")]
        public string BCH_PublicHealth { get; set; }

        [Name("City")]
        public string City { get; set; }

        [Name("COEHS Programs")]
        public string COEHSPrograms { get; set; }

        [Name("Department")]
        public string Department { get; set; }

        [Name("Email Address")]
        public string EmailAddress { get; set; }

        [Name("Faculty Initiator")]
        public string FacultyInitiator { get; set; }

        [Name("Graduate or Undergraduate")]
        public string Graduate_Undergraduate { get; set; }

        [Name("Phone Number")]
        public string PhoneNumber { get; set; }

        [Name("Primary Contact")]
        public string PrimaryContact { get; set; }

        [Name("Renewal")]
        public string Renewal { get; set; }

        [Name("State")]
        public string State { get; set; }

        [Name("Title Cert")]
        public string TitleCert { get; set; }

        [Name("Year")]
        public string Year { get; set; }

        [Name("Zip Code")]
        public string ZipCode { get; set; }

        [Ignore]
        public bool Error { get; set; }

        [Ignore]
        public string? ErrorDescription { get; set; }

        [Ignore]
        public bool Duplicate { get; set; } = false;
    }
}
