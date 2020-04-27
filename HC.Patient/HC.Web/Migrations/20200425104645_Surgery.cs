using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class Surgery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientSurgeon",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StaffId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSurgeon", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientSurgery",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(maxLength: 100, nullable: false),
                    SurgeryDateTime = table.Column<DateTime>(nullable: false),
                    SurgeryId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSurgery", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientSurgery_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientSurgery_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientSurgery_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientSurgeryImages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DoctorComments = table.Column<string>(nullable: true),
                    ImageDescription = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    ImagesType = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PatientComments = table.Column<string>(nullable: true),
                    SurgeryId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSurgeryImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientSurgeryImages_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientSurgeryImages_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientSurgeryImages_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientSurgery_CreatedBy",
                table: "PatientSurgery",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSurgery_DeletedBy",
                table: "PatientSurgery",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSurgery_UpdatedBy",
                table: "PatientSurgery",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSurgeryImages_CreatedBy",
                table: "PatientSurgeryImages",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSurgeryImages_DeletedBy",
                table: "PatientSurgeryImages",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSurgeryImages_UpdatedBy",
                table: "PatientSurgeryImages",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientSurgeon");

            migrationBuilder.DropTable(
                name: "PatientSurgery");

            migrationBuilder.DropTable(
                name: "PatientSurgeryImages");
        }
    }
}
