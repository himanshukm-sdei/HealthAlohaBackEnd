using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class mastercategorycoderenametoOptionLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoBase64",
                table: "MasterDFA_CategoryCode",
                newName: "OptionLogo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OptionLogo",
                table: "MasterDFA_CategoryCode",
                newName: "PhotoBase64");
        }
    }
}
