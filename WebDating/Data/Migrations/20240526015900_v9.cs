using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class v9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
