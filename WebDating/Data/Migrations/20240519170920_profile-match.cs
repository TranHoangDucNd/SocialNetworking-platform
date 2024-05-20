using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class profilematch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DatingAgeFrom",
                table: "DatingProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DatingAgeTo",
                table: "DatingProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatingAgeFrom",
                table: "DatingProfiles");

            migrationBuilder.DropColumn(
                name: "DatingAgeTo",
                table: "DatingProfiles");
        }
    }
}
