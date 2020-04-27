using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class sectionitem_rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterDFA_SectionItems");

            migrationBuilder.CreateTable(
                name: "MasterDFA_SectionItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ItemLabel = table.Column<string>(nullable: true),
                    Itemtype = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDFA_SectionItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterDFA_SectionItem_MasterDFA_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "MasterDFA_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterDFA_SectionItem_MasterDFA_CategoryCode_Itemtype",
                        column: x => x.Itemtype,
                        principalTable: "MasterDFA_CategoryCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterDFA_SectionItem_MasterDFA_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "MasterDFA_Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterDFA_SectionItem_CategoryId",
                table: "MasterDFA_SectionItem",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDFA_SectionItem_Itemtype",
                table: "MasterDFA_SectionItem",
                column: "Itemtype");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDFA_SectionItem_SectionId",
                table: "MasterDFA_SectionItem",
                column: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterDFA_SectionItem");

            migrationBuilder.CreateTable(
                name: "MasterDFA_SectionItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ItemLabel = table.Column<string>(nullable: true),
                    Itemtype = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDFA_SectionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterDFA_SectionItems_MasterDFA_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "MasterDFA_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterDFA_SectionItems_MasterDFA_CategoryCode_Itemtype",
                        column: x => x.Itemtype,
                        principalTable: "MasterDFA_CategoryCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterDFA_SectionItems_MasterDFA_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "MasterDFA_Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterDFA_SectionItems_CategoryId",
                table: "MasterDFA_SectionItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDFA_SectionItems_Itemtype",
                table: "MasterDFA_SectionItems",
                column: "Itemtype");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDFA_SectionItems_SectionId",
                table: "MasterDFA_SectionItems",
                column: "SectionId");
        }
    }
}
