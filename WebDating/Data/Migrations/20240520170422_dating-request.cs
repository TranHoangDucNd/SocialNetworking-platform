using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class datingrequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DatingRequestId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DatingRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    CrushId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ConfirmedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatingRequests_AspNetUsers_CrushId",
                        column: x => x.CrushId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DatingRequestId",
                table: "Notifications",
                column: "DatingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DatingRequests_CrushId",
                table: "DatingRequests",
                column: "CrushId");

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

            migrationBuilder.DropTable(
                name: "DatingRequests");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_DatingRequestId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DatingRequestId",
                table: "Notifications");
        }
    }
}
