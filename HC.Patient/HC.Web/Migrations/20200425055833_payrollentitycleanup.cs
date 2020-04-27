using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class payrollentitycleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_PayrollGroup_PayrollGroupID",
                table: "Staffs");

            migrationBuilder.DropTable(
                name: "PayrollBreakTimeDetails");

            migrationBuilder.DropTable(
                name: "PayrollGroup");

            migrationBuilder.DropTable(
                name: "PayrollBreakTime");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_PayrollGroupID",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "PayrollGroupID",
                table: "Staffs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PayrollGroupID",
                table: "Staffs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PayrollBreakTime",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollBreakTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTime_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTime_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTime_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTime_MasterState_StateId",
                        column: x => x.StateId,
                        principalTable: "MasterState",
                        principalColumn: "StateID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTime_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollBreakTimeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EndRange = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    NumberOfBreaks = table.Column<int>(nullable: false),
                    PayrollBreakTimeId = table.Column<int>(nullable: false),
                    StartRange = table.Column<decimal>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollBreakTimeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTimeDetails_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTimeDetails_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTimeDetails_PayrollBreakTime_PayrollBreakTimeId",
                        column: x => x.PayrollBreakTimeId,
                        principalTable: "PayrollBreakTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTimeDetails_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DailyLimit = table.Column<decimal>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DoubleOverTime = table.Column<decimal>(nullable: false),
                    GroupName = table.Column<string>(type: "varchar(200)", nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsCaliforniaRule = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    OverTime = table.Column<decimal>(nullable: false),
                    PayPeriodId = table.Column<int>(nullable: false),
                    PayrollBreakTimeId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    WeeklyLimit = table.Column<decimal>(nullable: false),
                    WorkWeekId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollGroup_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollGroup_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollGroup_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollGroup_GlobalCode_PayPeriodId",
                        column: x => x.PayPeriodId,
                        principalTable: "GlobalCode",
                        principalColumn: "GlobalCodeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollGroup_PayrollBreakTime_PayrollBreakTimeId",
                        column: x => x.PayrollBreakTimeId,
                        principalTable: "PayrollBreakTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollGroup_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollGroup_GlobalCode_WorkWeekId",
                        column: x => x.WorkWeekId,
                        principalTable: "GlobalCode",
                        principalColumn: "GlobalCodeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_PayrollGroupID",
                table: "Staffs",
                column: "PayrollGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTime_CreatedBy",
                table: "PayrollBreakTime",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTime_DeletedBy",
                table: "PayrollBreakTime",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTime_OrganizationId",
                table: "PayrollBreakTime",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTime_StateId",
                table: "PayrollBreakTime",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTime_UpdatedBy",
                table: "PayrollBreakTime",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTimeDetails_CreatedBy",
                table: "PayrollBreakTimeDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTimeDetails_DeletedBy",
                table: "PayrollBreakTimeDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTimeDetails_PayrollBreakTimeId",
                table: "PayrollBreakTimeDetails",
                column: "PayrollBreakTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTimeDetails_UpdatedBy",
                table: "PayrollBreakTimeDetails",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollGroup_CreatedBy",
                table: "PayrollGroup",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollGroup_DeletedBy",
                table: "PayrollGroup",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollGroup_OrganizationId",
                table: "PayrollGroup",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollGroup_PayPeriodId",
                table: "PayrollGroup",
                column: "PayPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollGroup_PayrollBreakTimeId",
                table: "PayrollGroup",
                column: "PayrollBreakTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollGroup_UpdatedBy",
                table: "PayrollGroup",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollGroup_WorkWeekId",
                table: "PayrollGroup",
                column: "WorkWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_PayrollGroup_PayrollGroupID",
                table: "Staffs",
                column: "PayrollGroupID",
                principalTable: "PayrollGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
