using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class updateSection_MasterQuest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HRAGenderCriteria",
                table: "MasterDFA_Section",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterDFA_Section_HRAGenderCriteria",
                table: "MasterDFA_Section",
                column: "HRAGenderCriteria");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDFA_Section_MasterGlobalCode_HRAGenderCriteria",
                table: "MasterDFA_Section",
                column: "HRAGenderCriteria",
                principalTable: "MasterGlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterDFA_Section_MasterGlobalCode_HRAGenderCriteria",
                table: "MasterDFA_Section");

            migrationBuilder.DropIndex(
                name: "IX_MasterDFA_Section_HRAGenderCriteria",
                table: "MasterDFA_Section");

            migrationBuilder.DropColumn(
                name: "HRAGenderCriteria",
                table: "MasterDFA_Section");
        }
    }
}
