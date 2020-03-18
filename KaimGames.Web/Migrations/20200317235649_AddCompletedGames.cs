using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KaimGames.Web.Migrations
{
    public partial class AddCompletedGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompletedGames",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Completed = table.Column<DateTime>(nullable: false),
                    GameName = table.Column<string>(nullable: true),
                    Elapsed = table.Column<double>(nullable: false),
                    SubGame = table.Column<string>(nullable: true),
                    Moves = table.Column<int>(nullable: false),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedGames_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedGames_UserId",
                table: "CompletedGames",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedGames");
        }
    }
}
