using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIPS.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
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
                    table.PrimaryKey("PK_Contracts", x => x.ContractID);
                });

            migrationBuilder.CreateTable(
                name: "CSVs",
                columns: table => new
                {
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
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Error = table.Column<bool>(type: "bit", nullable: false),
                    ErrorDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duplicate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSVs", x => x.ContractID);
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    PartnerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.PartnerId);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.SchoolId);
                });

            migrationBuilder.CreateTable(
                name: "Edges",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edges", x => new { x.ContractId, x.FromId, x.ToId });
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    NodeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    x = table.Column<int>(type: "int", nullable: false),
                    y = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => new { x.NodeId });
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_Departments_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "SchoolId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SchoolId",
                table: "Departments",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "CSVs");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "Edges");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}
