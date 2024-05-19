using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDatingRequestIdToNotify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DatingRequestId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DatingRequestId",
                table: "Notifications",
                column: "DatingRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_DatingRequests_DatingRequestId",
                table: "Notifications",
                column: "DatingRequestId",
                principalTable: "DatingRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_DatingRequests_DatingRequestId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_DatingRequestId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DatingRequestId",
                table: "Notifications");
        }
    }
}
