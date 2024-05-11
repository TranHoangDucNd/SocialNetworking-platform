using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class SplitFKCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Comments_TargetId",
                table: "ReactionLogs");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                table: "ReactionLogs",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_ReactionLogs_TargetId",
                table: "ReactionLogs",
                newName: "IX_ReactionLogs_PostId");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "ReactionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionLogs_Post_PostId",
                table: "ReactionLogs",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Post_PostId",
                table: "ReactionLogs");

            migrationBuilder.DropIndex(
                name: "IX_ReactionLogs_CommentId",
                table: "ReactionLogs");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "ReactionLogs");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "ReactionLogs",
                newName: "TargetId");

            migrationBuilder.RenameIndex(
                name: "IX_ReactionLogs_PostId",
                table: "ReactionLogs",
                newName: "IX_ReactionLogs_TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionLogs_Comments_TargetId",
                table: "ReactionLogs",
                column: "TargetId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
