using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSomething : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatingRequest_AspNetUsers_UserId2",
                table: "DatingRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DatingRequest",
                table: "DatingRequest");

            migrationBuilder.DropIndex(
                name: "IX_DatingRequest_UserId2",
                table: "DatingRequest");

            migrationBuilder.RenameTable(
                name: "DatingRequest",
                newName: "DatingRequests");

            migrationBuilder.RenameColumn(
                name: "UserId2",
                table: "DatingRequests",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "DatingRequests",
                newName: "CrushId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DatingRequests",
                table: "DatingRequests",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DatingRequests_CrushId",
                table: "DatingRequests",
                column: "CrushId");

            migrationBuilder.AddForeignKey(
                name: "FK_DatingRequests_AspNetUsers_CrushId",
                table: "DatingRequests",
                column: "CrushId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatingRequests_AspNetUsers_CrushId",
                table: "DatingRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DatingRequests",
                table: "DatingRequests");

            migrationBuilder.DropIndex(
                name: "IX_DatingRequests_CrushId",
                table: "DatingRequests");

            migrationBuilder.RenameTable(
                name: "DatingRequests",
                newName: "DatingRequest");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "DatingRequest",
                newName: "UserId2");

            migrationBuilder.RenameColumn(
                name: "CrushId",
                table: "DatingRequest",
                newName: "UserId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DatingRequest",
                table: "DatingRequest",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DatingRequest_UserId2",
                table: "DatingRequest",
                column: "UserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_DatingRequest_AspNetUsers_UserId2",
                table: "DatingRequest",
                column: "UserId2",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
