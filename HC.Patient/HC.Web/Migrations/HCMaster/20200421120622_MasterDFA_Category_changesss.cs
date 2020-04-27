using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class MasterDFA_Category_changesss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterDFA_Categories_MasterOrganization_OrganizationID",
                table: "MasterDFA_Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterDFA_CategoryCode_MasterDFA_Categories_CategoryId",
                table: "MasterDFA_CategoryCode");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterDFA_SectionItem_MasterDFA_Categories_CategoryId",
                table: "MasterDFA_SectionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterMappingHRACategoryRisk_MasterDFA_Categories_HRACategoryId",
                table: "MasterMappingHRACategoryRisk");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MasterDFA_Categories",
                table: "MasterDFA_Categories");

            migrationBuilder.RenameTable(
                name: "MasterDFA_Categories",
                newName: "MasterDFA_Category");

            migrationBuilder.RenameIndex(
                name: "IX_MasterDFA_Categories_OrganizationID",
                table: "MasterDFA_Category",
                newName: "IX_MasterDFA_Category_OrganizationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MasterDFA_Category",
                table: "MasterDFA_Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDFA_Category_MasterOrganization_OrganizationID",
                table: "MasterDFA_Category",
                column: "OrganizationID",
                principalTable: "MasterOrganization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDFA_CategoryCode_MasterDFA_Category_CategoryId",
                table: "MasterDFA_CategoryCode",
                column: "CategoryId",
                principalTable: "MasterDFA_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDFA_SectionItem_MasterDFA_Category_CategoryId",
                table: "MasterDFA_SectionItem",
                column: "CategoryId",
                principalTable: "MasterDFA_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterMappingHRACategoryRisk_MasterDFA_Category_HRACategoryId",
                table: "MasterMappingHRACategoryRisk",
                column: "HRACategoryId",
                principalTable: "MasterDFA_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterDFA_Category_MasterOrganization_OrganizationID",
                table: "MasterDFA_Category");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterDFA_CategoryCode_MasterDFA_Category_CategoryId",
                table: "MasterDFA_CategoryCode");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterDFA_SectionItem_MasterDFA_Category_CategoryId",
                table: "MasterDFA_SectionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterMappingHRACategoryRisk_MasterDFA_Category_HRACategoryId",
                table: "MasterMappingHRACategoryRisk");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MasterDFA_Category",
                table: "MasterDFA_Category");

            migrationBuilder.RenameTable(
                name: "MasterDFA_Category",
                newName: "MasterDFA_Categories");

            migrationBuilder.RenameIndex(
                name: "IX_MasterDFA_Category_OrganizationID",
                table: "MasterDFA_Categories",
                newName: "IX_MasterDFA_Categories_OrganizationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MasterDFA_Categories",
                table: "MasterDFA_Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDFA_Categories_MasterOrganization_OrganizationID",
                table: "MasterDFA_Categories",
                column: "OrganizationID",
                principalTable: "MasterOrganization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDFA_CategoryCode_MasterDFA_Categories_CategoryId",
                table: "MasterDFA_CategoryCode",
                column: "CategoryId",
                principalTable: "MasterDFA_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDFA_SectionItem_MasterDFA_Categories_CategoryId",
                table: "MasterDFA_SectionItem",
                column: "CategoryId",
                principalTable: "MasterDFA_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterMappingHRACategoryRisk_MasterDFA_Categories_HRACategoryId",
                table: "MasterMappingHRACategoryRisk",
                column: "HRACategoryId",
                principalTable: "MasterDFA_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
