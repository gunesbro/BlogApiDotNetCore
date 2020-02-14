using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogProjectAPI.Migrations
{
    public partial class AccesLogUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenAccessLogs");

            migrationBuilder.CreateTable(
                name: "AccessLogs",
                columns: table => new
                {
                    AccessLogsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    RequestOrResponse = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    QueryString = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    Scheme = table.Column<string>(nullable: true),
                    WhoRequested = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLogs", x => x.AccessLogsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessLogs");

            migrationBuilder.CreateTable(
                name: "TokenAccessLogs",
                columns: table => new
                {
                    TokenAccessLogsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Access = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessRequest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessTrueFalse = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenAccessLogs", x => x.TokenAccessLogsId);
                });
        }
    }
}
