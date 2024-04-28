using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DatingProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdatedDatingProfile",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DatingProfileId",
                table: "AspNetUsers",
                column: "DatingProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DatingProfiles_DatingProfileId",
                table: "AspNetUsers",
                column: "DatingProfileId",
                principalTable: "DatingProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DatingProfiles_DatingProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DatingProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DatingProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsUpdatedDatingProfile",
                table: "AspNetUsers");
        }
    }
}
