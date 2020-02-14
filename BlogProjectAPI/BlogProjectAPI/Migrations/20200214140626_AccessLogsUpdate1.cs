using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogProjectAPI.Migrations
{
    public partial class AccessLogsUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "AccessLogs");

            migrationBuilder.DropColumn(
                name: "RequestOrResponse",
                table: "AccessLogs");

            migrationBuilder.AddColumn<string>(
                name: "RequestBody",
                table: "AccessLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseBody",
                table: "AccessLogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "AccessLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestBody",
                table: "AccessLogs");

            migrationBuilder.DropColumn(
                name: "ResponseBody",
                table: "AccessLogs");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "AccessLogs");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "AccessLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestOrResponse",
                table: "AccessLogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
