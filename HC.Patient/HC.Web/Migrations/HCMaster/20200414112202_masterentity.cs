using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class masterentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumenttypeId",
                table: "MasterDFA_Document",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MasterAssessmentTypeId",
                table: "MasterDFA_Document",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MasterAssessmentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssessmentName = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterAssessmentType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterAssessmentType_MasterOrganization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "MasterOrganization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterBenchmark",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: true),
                    OrganizationId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterBenchmark", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterBenchmark_MasterOrganization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "MasterOrganization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterDiseaseManagementProgram",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDiseaseManagementProgram", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MasterDiseaseManagementProgram_MasterOrganization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "MasterOrganization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterQuestionnaireBenchmarkRange",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BenchmarkId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MaxRange = table.Column<decimal>(nullable: false),
                    MinRange = table.Column<decimal>(nullable: false),
                    QuestionnaireId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterQuestionnaireBenchmarkRange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterQuestionnaireBenchmarkRange_MasterBenchmark_BenchmarkId",
                        column: x => x.BenchmarkId,
                        principalTable: "MasterBenchmark",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterQuestionnaireBenchmarkRange_MasterDFA_Document_QuestionnaireId",
                        column: x => x.QuestionnaireId,
                        principalTable: "MasterDFA_Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterDFA_Document_DocumenttypeId",
                table: "MasterDFA_Document",
                column: "DocumenttypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDFA_Document_MasterAssessmentTypeId",
                table: "MasterDFA_Document",
                column: "MasterAssessmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterAssessmentType_OrganizationId",
                table: "MasterAssessmentType",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterBenchmark_OrganizationId",
                table: "MasterBenchmark",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDiseaseManagementProgram_OrganizationId",
                table: "MasterDiseaseManagementProgram",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterQuestionnaireBenchmarkRange_BenchmarkId",
                table: "MasterQuestionnaireBenchmarkRange",
                column: "BenchmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterQuestionnaireBenchmarkRange_QuestionnaireId",
                table: "MasterQuestionnaireBenchmarkRange",
                column: "QuestionnaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDFA_Document_MasterDiseaseManagementProgram_DocumenttypeId",
                table: "MasterDFA_Document",
                column: "DocumenttypeId",
                principalTable: "MasterDiseaseManagementProgram",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDFA_Document_MasterAssessmentType_MasterAssessmentTypeId",
                table: "MasterDFA_Document",
                column: "MasterAssessmentTypeId",
                principalTable: "MasterAssessmentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterDFA_Document_MasterDiseaseManagementProgram_DocumenttypeId",
                table: "MasterDFA_Document");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterDFA_Document_MasterAssessmentType_MasterAssessmentTypeId",
                table: "MasterDFA_Document");

            migrationBuilder.DropTable(
                name: "MasterAssessmentType");

            migrationBuilder.DropTable(
                name: "MasterDiseaseManagementProgram");

            migrationBuilder.DropTable(
                name: "MasterQuestionnaireBenchmarkRange");

            migrationBuilder.DropTable(
                name: "MasterBenchmark");

            migrationBuilder.DropIndex(
                name: "IX_MasterDFA_Document_DocumenttypeId",
                table: "MasterDFA_Document");

            migrationBuilder.DropIndex(
                name: "IX_MasterDFA_Document_MasterAssessmentTypeId",
                table: "MasterDFA_Document");

            migrationBuilder.DropColumn(
                name: "DocumenttypeId",
                table: "MasterDFA_Document");

            migrationBuilder.DropColumn(
                name: "MasterAssessmentTypeId",
                table: "MasterDFA_Document");
        }
    }
}
