using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class v17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Post_PostId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Post_PostId",
                table: "Notifications",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Post_PostId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Post_PostId",
                table: "Notifications",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
