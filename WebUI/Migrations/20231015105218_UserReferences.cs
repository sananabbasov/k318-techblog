using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebUI.Migrations
{
    /// <inheritdoc />
    public partial class UserReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UserId",
                table: "Articles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Users_UserId1",
                table: "Articles",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Users_UserId1",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_UserId1",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Articles");
        }
    }
}
