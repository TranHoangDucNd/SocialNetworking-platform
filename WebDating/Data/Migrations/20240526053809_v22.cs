using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class v22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__PostRepor__PostI__17036CC0",
                table: "PostReportDetail");

            migrationBuilder.AddForeignKey(
                name: "FK__PostRepor__PostI__17036CC0",
                table: "PostReportDetail",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__PostRepor__PostI__17036CC0",
                table: "PostReportDetail");

            migrationBuilder.AddForeignKey(
                name: "FK__PostRepor__PostI__17036CC0",
                table: "PostReportDetail",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
