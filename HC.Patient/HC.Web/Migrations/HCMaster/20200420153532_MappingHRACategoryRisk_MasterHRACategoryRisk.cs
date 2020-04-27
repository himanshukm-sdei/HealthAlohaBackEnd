using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class MappingHRACategoryRisk_MasterHRACategoryRisk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterHRACategoryRisk",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ExecutiveDescription = table.Column<string>(nullable: true),
                    HRAGenderCriteria = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: true),
                    ReferralLinks = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterHRACategoryRisk", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MasterHRACategoryRisk_MasterGlobalCode_HRAGenderCriteria",
                        column: x => x.HRAGenderCriteria,
                        principalTable: "MasterGlobalCode",
                        principalColumn: "GlobalCodeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterHRACategoryRisk_MasterOrganization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "MasterOrganization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterHRACategoryRiskBenchmarkMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MasterBenchmarkId = table.Column<int>(nullable: true),
                    MasterHRACategoryRiskId = table.Column<int>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterHRACategoryRiskBenchmarkMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterHRACategoryRiskBenchmarkMapping_MasterBenchmark_MasterBenchmarkId",
                        column: x => x.MasterBenchmarkId,
                        principalTable: "MasterBenchmark",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterHRACategoryRiskBenchmarkMapping_MasterHRACategoryRisk_MasterHRACategoryRiskId",
                        column: x => x.MasterHRACategoryRiskId,
                        principalTable: "MasterHRACategoryRisk",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterMappingHRACategoryRisk",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HRACategoryId = table.Column<int>(nullable: false),
                    HRACategoryRiskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterMappingHRACategoryRisk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterMappingHRACategoryRisk_MasterDFA_Category_HRACategoryId",
                        column: x => x.HRACategoryId,
                        principalTable: "MasterDFA_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterMappingHRACategoryRisk_MasterHRACategoryRisk_HRACategoryRiskId",
                        column: x => x.HRACategoryRiskId,
                        principalTable: "MasterHRACategoryRisk",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterHRACategoryRisk_HRAGenderCriteria",
                table: "MasterHRACategoryRisk",
                column: "HRAGenderCriteria");

            migrationBuilder.CreateIndex(
                name: "IX_MasterHRACategoryRisk_OrganizationId",
                table: "MasterHRACategoryRisk",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterHRACategoryRiskBenchmarkMapping_MasterBenchmarkId",
                table: "MasterHRACategoryRiskBenchmarkMapping",
                column: "MasterBenchmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterHRACategoryRiskBenchmarkMapping_MasterHRACategoryRiskId",
                table: "MasterHRACategoryRiskBenchmarkMapping",
                column: "MasterHRACategoryRiskId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterMappingHRACategoryRisk_HRACategoryId",
                table: "MasterMappingHRACategoryRisk",
                column: "HRACategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterMappingHRACategoryRisk_HRACategoryRiskId",
                table: "MasterMappingHRACategoryRisk",
                column: "HRACategoryRiskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterHRACategoryRiskBenchmarkMapping");

            migrationBuilder.DropTable(
                name: "MasterMappingHRACategoryRisk");

            migrationBuilder.DropTable(
                name: "MasterHRACategoryRisk");
        }
    }
}
