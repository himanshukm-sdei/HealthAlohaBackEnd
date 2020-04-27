using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddNewColumnInVideoAndArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DaysBeforeAndAfter",
                table: "VideoAndArticles",
                newName: "Days");

            migrationBuilder.AddColumn<string>(
                name: "BeforeAndAfter",
                table: "VideoAndArticles",
                type: "varchar(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeforeAndAfter",
                table: "VideoAndArticles");

            migrationBuilder.RenameColumn(
                name: "Days",
                table: "VideoAndArticles",
                newName: "DaysBeforeAndAfter");
        }
    }
}
