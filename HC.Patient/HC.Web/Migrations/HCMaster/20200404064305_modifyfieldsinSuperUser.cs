using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class modifyfieldsinSuperUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SuperUser",
                newName: "Id");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SuperUser",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "SuperUser",
                nullable: true,
                defaultValueSql: "GetUtcDate()",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GetUtcDate()");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "SuperUser",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "SuperUser",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "SuperUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SuperUser");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "SuperUser");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SuperUser",
                newName: "ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SuperUser",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "SuperUser",
                nullable: false,
                defaultValueSql: "GetUtcDate()",
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "GetUtcDate()");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "SuperUser",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
