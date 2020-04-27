using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class ArticleAndVideoTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleAndVideos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryID = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DaysBeforeAndAfter = table.Column<int>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Image = table.Column<string>(type: "varchar(100)", nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsVideo = table.Column<bool>(nullable: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    ShortDescription = table.Column<string>(type: "varchar(500)", nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    Url = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleAndVideos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArticleAndVideos_CategoriesArticleAndVideo_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "CategoriesArticleAndVideo",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleAndVideos_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleAndVideos_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleAndVideos_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleAndVideos_CategoryID",
                table: "ArticleAndVideos",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleAndVideos_CreatedBy",
                table: "ArticleAndVideos",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleAndVideos_DeletedBy",
                table: "ArticleAndVideos",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleAndVideos_UpdatedBy",
                table: "ArticleAndVideos",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleAndVideos");
        }
    }
}
