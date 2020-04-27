using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class RenameCheckListTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SAD_CheckListCategory",
                table: "SAD_CheckListCategory");

            migrationBuilder.RenameTable(
                name: "SAD_CheckListCategory",
                newName: "MasterCheckListCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MasterCheckListCategory",
                table: "MasterCheckListCategory",
                column: "CheckListCategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MasterCheckListCategory",
                table: "MasterCheckListCategory");

            migrationBuilder.RenameTable(
                name: "MasterCheckListCategory",
                newName: "SAD_CheckListCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SAD_CheckListCategory",
                table: "SAD_CheckListCategory",
                column: "CheckListCategoryID");
        }
    }
}
