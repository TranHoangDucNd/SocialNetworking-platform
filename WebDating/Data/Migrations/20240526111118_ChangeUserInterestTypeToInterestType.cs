using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserInterestTypeToInterestType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "UserInterests",
                newName: "InterestType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InterestType",
                table: "UserInterests",
                newName: "Type");
        }
    }
}
