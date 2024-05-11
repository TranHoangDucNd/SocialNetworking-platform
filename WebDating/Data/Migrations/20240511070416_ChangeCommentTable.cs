using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs");

            migrationBuilder.DropIndex(
                name: "IX_ReactionLogs_CommentId",
                table: "ReactionLogs");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "ReactionLogs",
                newName: "TargetId");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "ReactionLogs",
                newName: "Target");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionLogs_TargetId",
                table: "ReactionLogs",
                column: "TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionLogs_Comments_TargetId",
                table: "ReactionLogs",
                column: "TargetId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Comments_TargetId",
                table: "ReactionLogs");

            migrationBuilder.DropIndex(
                name: "IX_ReactionLogs_TargetId",
                table: "ReactionLogs");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                table: "ReactionLogs",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "Target",
                table: "ReactionLogs",
                newName: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionLogs_CommentId",
                table: "ReactionLogs",
                column: "CommentId");

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
