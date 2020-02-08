using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogProjectAPI.Migrations
{
    public partial class AzureInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorUsername = table.Column<string>(nullable: true),
                    AuthorPassword = table.Column<string>(nullable: true),
                    AuthorName = table.Column<string>(nullable: true),
                    AuthorSurname = table.Column<string>(nullable: true),
                    AuthorAvatar = table.Column<string>(nullable: true),
                    AuthorSortDesc = table.Column<string>(nullable: true),
                    AuthorCreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionLogs",
                columns: table => new
                {
                    ExceptionLogsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExceptionFile = table.Column<string>(nullable: true),
                    ExceptionMethod = table.Column<string>(nullable: true),
                    ExceptionLine = table.Column<string>(nullable: true),
                    ExceptionMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLogs", x => x.ExceptionLogsId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(nullable: true),
                    RoleDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "TokenAccessLogs",
                columns: table => new
                {
                    TokenAccessLogsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Access = table.Column<string>(nullable: true),
                    AccessTrueFalse = table.Column<bool>(nullable: false),
                    AccessRequest = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenAccessLogs", x => x.TokenAccessLogsId);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostTitle = table.Column<string>(nullable: true),
                    PostContent = table.Column<string>(nullable: true),
                    PostDesc = table.Column<string>(nullable: true),
                    PostCoverImage = table.Column<string>(nullable: true),
                    PostCreationDate = table.Column<DateTime>(nullable: false),
                    PostPublishDate = table.Column<DateTime>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    PostUpdateDate = table.Column<DateTime>(nullable: true),
                    LastUpdatedAuthorId = table.Column<int>(nullable: false),
                    AuthorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TokenUsers",
                columns: table => new
                {
                    TokenUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    IsSuspended = table.Column<bool>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    UserDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenUsers", x => x.TokenUserId);
                    table.ForeignKey(
                        name: "FK_TokenUsers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagsOfPosts",
                columns: table => new
                {
                    TagsOfPostsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagsOfPosts", x => x.TagsOfPostsId);
                    table.ForeignKey(
                        name: "FK_TagsOfPosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagsOfPosts_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_TagsOfPosts_PostId",
                table: "TagsOfPosts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_TagsOfPosts_TagId",
                table: "TagsOfPosts",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenUsers_RoleId",
                table: "TokenUsers",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionLogs");

            migrationBuilder.DropTable(
                name: "TagsOfPosts");

            migrationBuilder.DropTable(
                name: "TokenAccessLogs");

            migrationBuilder.DropTable(
                name: "TokenUsers");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
