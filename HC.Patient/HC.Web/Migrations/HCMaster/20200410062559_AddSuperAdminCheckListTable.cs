using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class AddSuperAdminCheckListTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SAD_CheckList",
                columns: table => new
                {
                    CheckListID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CheckListCategoryID = table.Column<int>(nullable: false),
                    CheckListPoints = table.Column<string>(type: "varchar(500)", nullable: true),
                    CheckListPointsOrder = table.Column<int>(nullable: false),
                    CheckListType = table.Column<string>(type: "varchar(4)", nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DaysBeforeAndAfter = table.Column<int>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAD_CheckList", x => x.CheckListID);
                    table.ForeignKey(
                        name: "FK_SAD_CheckList_SAD_CheckListCategory_CheckListCategoryID",
                        column: x => x.CheckListCategoryID,
                        principalTable: "SAD_CheckListCategory",
                        principalColumn: "CheckListCategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SAD_CheckList_CheckListCategoryID",
                table: "SAD_CheckList",
                column: "CheckListCategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAD_CheckList");
        }
    }
}
