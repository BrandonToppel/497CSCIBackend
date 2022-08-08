using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereToWatchAPI.Migrations
{
    public partial class AddSchemeaforsearchhistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Search = table.Column<string>(nullable: true),
                    SearchDate = table.Column<DateTime>(nullable: false),
                    SearchNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSearch",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    SearchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSearch", x => new { x.UserId, x.SearchId });
                    table.ForeignKey(
                        name: "FK_UserSearch_SearchHistories_SearchId",
                        column: x => x.SearchId,
                        principalTable: "SearchHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSearch_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSearch_SearchId",
                table: "UserSearch",
                column: "SearchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSearch");

            migrationBuilder.DropTable(
                name: "SearchHistories");
        }
    }
}
