using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddCheckList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckList",
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
                    table.PrimaryKey("PK_CheckList", x => x.CheckListID);
                    table.ForeignKey(
                        name: "FK_CheckList_CheckListCategory_CheckListCategoryID",
                        column: x => x.CheckListCategoryID,
                        principalTable: "CheckListCategory",
                        principalColumn: "CheckListCategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckList_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckList_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckList_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckList_CheckListCategoryID",
                table: "CheckList",
                column: "CheckListCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckList_CreatedBy",
                table: "CheckList",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CheckList_DeletedBy",
                table: "CheckList",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CheckList_UpdatedBy",
                table: "CheckList",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckList");
        }
    }
}
