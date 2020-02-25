using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace QMS.Migrations.PatientDb
{
    public partial class PatientInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Patient",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FHIR_id = table.Column<int>(type: "int4", nullable: true),
                    HIVtest = table.Column<bool>(type: "bool", nullable: false),
                    IsPregnant = table.Column<bool>(type: "bool", nullable: false),
                    abused = table.Column<bool>(type: "bool", nullable: false),
                    active = table.Column<bool>(type: "bool", nullable: false),
                    birthDate = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    complaint = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    firstname = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    gender = table.Column<int>(type: "int4", nullable: false),
                    lastname = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    lifethreatening = table.Column<bool>(type: "bool", nullable: false),
                    line1 = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    line2 = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    policy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    postalCode = table.Column<string>(type: "text", nullable: false),
                    preferredGender = table.Column<int>(type: "int4", nullable: false),
                    prefgender = table.Column<string>(type: "text", nullable: true),
                    primary = table.Column<bool>(type: "bool", nullable: false),
                    primaryName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    primaryPhysician = table.Column<bool>(type: "bool", nullable: false),
                    provider = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    state = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    telecom = table.Column<string>(type: "text", nullable: true),
                    travel = table.Column<bool>(type: "bool", nullable: false),
                    vaccines = table.Column<bool>(type: "bool", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Encounter",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FHIR_id = table.Column<int>(type: "int4", nullable: true),
                    end = table.Column<DateTime>(type: "timestamp", nullable: true),
                    patient_id = table.Column<int>(type: "int4", nullable: false),
                    start = table.Column<DateTime>(type: "timestamp", nullable: false),
                    status = table.Column<int>(type: "int4", nullable: false),
                    visitType = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounter", x => x.id);
                    table.ForeignKey(
                        name: "FK_Encounter_Patient_patient_id",
                        column: x => x.patient_id,
                        principalSchema: "public",
                        principalTable: "Patient",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Encounter_patient_id",
                schema: "public",
                table: "Encounter",
                column: "patient_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Encounter",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Patient",
                schema: "public");
        }
    }
}
