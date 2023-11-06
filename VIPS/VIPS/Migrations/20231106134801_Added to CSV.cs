using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIPS.Migrations
{
    /// <inheritdoc />
    public partial class AddedtoCSV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ErrorDescription",
                table: "CSVs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "Duplicate",
                table: "CSVs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duplicate",
                table: "CSVs");

            migrationBuilder.AlterColumn<bool>(
                name: "ErrorDescription",
                table: "CSVs",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
