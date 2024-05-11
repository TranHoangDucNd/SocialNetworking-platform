using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDating.Data.Migrations
{
    /// <inheritdoc />
    public partial class TestConstrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Post_PostId",
                table: "ReactionLogs");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "ReactionLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "ReactionLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddCheckConstraint(
                name: "CheckForeignKeyCount",
                table: "ReactionLogs",
                sql: "(CommentId IS NOT NULL AND PostId IS NULL) OR (CommentId IS NULL AND PostId IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");

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
                name: "FK_ReactionLogs_Comments_CommentId",
                table: "ReactionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionLogs_Post_PostId",
                table: "ReactionLogs");

            migrationBuilder.DropCheckConstraint(
                name: "CheckForeignKeyCount",
                table: "ReactionLogs");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "ReactionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "ReactionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
