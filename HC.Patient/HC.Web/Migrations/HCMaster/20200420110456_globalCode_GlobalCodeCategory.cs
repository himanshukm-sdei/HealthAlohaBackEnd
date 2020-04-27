using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class globalCode_GlobalCodeCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterGlobalCodeCategory",
                columns: table => new
                {
                    GlobalCodeCategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    GlobalCodeCategoryName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterGlobalCodeCategory", x => x.GlobalCodeCategoryID);
                    table.ForeignKey(
                        name: "FK_MasterGlobalCodeCategory_MasterOrganization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "MasterOrganization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterGlobalCode",
                columns: table => new
                {
                    GlobalCodeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: true),
                    GlobalCodeCategoryID = table.Column<int>(nullable: false),
                    GlobalCodeName = table.Column<string>(nullable: true),
                    GlobalCodeValue = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterGlobalCode", x => x.GlobalCodeID);
                    table.ForeignKey(
                        name: "FK_MasterGlobalCode_MasterGlobalCodeCategory_GlobalCodeCategoryID",
                        column: x => x.GlobalCodeCategoryID,
                        principalTable: "MasterGlobalCodeCategory",
                        principalColumn: "GlobalCodeCategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterGlobalCode_MasterOrganization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "MasterOrganization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterGlobalCode_GlobalCodeCategoryID",
                table: "MasterGlobalCode",
                column: "GlobalCodeCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterGlobalCode_OrganizationID",
                table: "MasterGlobalCode",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterGlobalCodeCategory_OrganizationID",
                table: "MasterGlobalCodeCategory",
                column: "OrganizationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterGlobalCode");

            migrationBuilder.DropTable(
                name: "MasterGlobalCodeCategory");
        }
    }
}
