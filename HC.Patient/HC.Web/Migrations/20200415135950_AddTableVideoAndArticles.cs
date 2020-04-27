using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddTableVideoAndArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoAndArticles",
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
                    table.PrimaryKey("PK_VideoAndArticles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VideoAndArticles_VideoAndArticlesCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "VideoAndArticlesCategories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoAndArticles_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoAndArticles_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoAndArticles_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoAndArticles_CategoryID",
                table: "VideoAndArticles",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoAndArticles_CreatedBy",
                table: "VideoAndArticles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VideoAndArticles_DeletedBy",
                table: "VideoAndArticles",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VideoAndArticles_UpdatedBy",
                table: "VideoAndArticles",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoAndArticles");
        }
    }
}
