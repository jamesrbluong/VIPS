using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSVParser.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CSVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractID = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractOrigin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentStageAssignees = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DaysInCurrStage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalContractReferenceID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Locked = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryDocument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedToContract = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedToContractID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedOn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Workflow = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgramsOrCourses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CCECMajors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoRenewal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgencyMailingAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgencyMailingAddress2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_AgingServicesManagement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_AthleticTraining = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_College = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_ExerciseScience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_HealthAdministration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_InterdisciplinaryHealthStudies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_MentalHealthCounseling = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_NurseAnesthetist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_Nursing = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_NutritionDietetics = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_PhysicalTherapy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCH_PublicHealth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    COEHSPrograms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacultyInitiator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Graduate_Undergraduate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Renewal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleCert = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSVs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CSVs");
        }
    }
}
