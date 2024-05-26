using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class v18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Post_PostId",
                table: "ReactionLogs");

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
                name: "FK_ReactionLogs_Post_PostId",
                table: "ReactionLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionLogs_Post_PostId",
                table: "ReactionLogs",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }
    }
}
