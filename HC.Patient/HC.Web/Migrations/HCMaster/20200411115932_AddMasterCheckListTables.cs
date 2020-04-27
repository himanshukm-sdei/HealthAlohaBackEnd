using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class AddMasterCheckListTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterCheckList",
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
                    table.PrimaryKey("PK_MasterCheckList", x => x.CheckListID);
                    table.ForeignKey(
                        name: "FK_MasterCheckList_MasterCheckListCategory_CheckListCategoryID",
                        column: x => x.CheckListCategoryID,
                        principalTable: "MasterCheckListCategory",
                        principalColumn: "CheckListCategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterCheckList_CheckListCategoryID",
                table: "MasterCheckList",
                column: "CheckListCategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterCheckList");
        }
    }
}
