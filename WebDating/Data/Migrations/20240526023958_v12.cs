using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class v12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Post_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_NotifyToUserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK__PostRepor__PostI__17036CC0",
                table: "PostReportDetail");

            migrationBuilder.DropCheckConstraint(
                name: "CheckForeignKeyCount",
                table: "ReactionLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Post_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_NotifyToUserId",
                table: "Notifications",
                column: "NotifyToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__PostRepor__PostI__17036CC0",
                table: "PostReportDetail",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Post_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_NotifyToUserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK__PostRepor__PostI__17036CC0",
                table: "PostReportDetail");

            migrationBuilder.AddCheckConstraint(
                name: "CheckForeignKeyCount",
                table: "ReactionLogs",
                sql: "(CommentId IS NOT NULL AND PostId IS NULL) OR (CommentId IS NULL AND PostId IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Post_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_NotifyToUserId",
                table: "Notifications",
                column: "NotifyToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__PostRepor__PostI__17036CC0",
                table: "PostReportDetail",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }
    }
}
