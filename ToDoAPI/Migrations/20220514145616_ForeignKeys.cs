using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoAPI.Migrations
{
    public partial class ForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ToDoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoes_UserId",
                table: "ToDoes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoes_Users_UserId",
                table: "ToDoes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoes_Users_UserId",
                table: "ToDoes");

            migrationBuilder.DropIndex(
                name: "IX_ToDoes_UserId",
                table: "ToDoes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ToDoes");
        }
    }
}
