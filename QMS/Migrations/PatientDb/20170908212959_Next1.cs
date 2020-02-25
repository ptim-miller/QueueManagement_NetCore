using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace QMS.Migrations.PatientDb
{
    public partial class Next1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "prefgender",
                schema: "public",
                table: "Patient");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "prefgender",
                schema: "public",
                table: "Patient",
                nullable: true);
        }
    }
}
