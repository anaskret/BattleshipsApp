using Microsoft.EntityFrameworkCore.Migrations;

namespace Battleships.DataAccess.Migrations
{
    public partial class player : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayerTwoId",
                table: "Lobbies",
                newName: "PlayerTwo");

            migrationBuilder.RenameColumn(
                name: "PlayerOneId",
                table: "Lobbies",
                newName: "PlayerOne");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayerTwo",
                table: "Lobbies",
                newName: "PlayerTwoId");

            migrationBuilder.RenameColumn(
                name: "PlayerOne",
                table: "Lobbies",
                newName: "PlayerOneId");
        }
    }
}
