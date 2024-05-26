using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class v20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Post_PostId",
                table: "ReactionLogs");

            migrationBuilder.AddCheckConstraint(
                name: "CheckForeignKeyCount",
                table: "ReactionLogs",
                sql: "(CommentId IS NOT NULL AND PostId IS NULL) OR (CommentId IS NULL AND PostId IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionLogs_Post_PostId",
                table: "ReactionLogs",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Post_PostId",
                table: "ReactionLogs");

            migrationBuilder.DropCheckConstraint(
                name: "CheckForeignKeyCount",
                table: "ReactionLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionLogs_Post_PostId",
                table: "ReactionLogs",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
