using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TaskManagementAPI.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    TaskGuid = table.Column<Guid>(nullable: false),
                    TaskNum = table.Column<int>(nullable: false, defaultValueSql: "nextval('seq_task_tasknum')"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Explanation = table.Column<string>(nullable: true),
                    ReturnDataType = table.Column<string>(nullable: false),
                    MethodName = table.Column<string>(nullable: false),
                    FirstInputParameterDataType = table.Column<string>(nullable: false),
                    SecondInputParameterDataType = table.Column<string>(nullable: true),
                    IsProdcution = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                    table.UniqueConstraint("AK_Tasks_TaskGuid", x => x.TaskGuid);
                });

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    CaseId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CaseNum = table.Column<int>(nullable: false, defaultValueSql: "nextval('seq_case_casenum')"),
                    TaskGuid = table.Column<Guid>(nullable: false),
                    FirstInputParameter = table.Column<string>(nullable: false),
                    SecondInputParameter = table.Column<string>(nullable: true),
                    ValidReturnValue = table.Column<string>(nullable: false),
                    SecretTest = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.CaseId);
                    table.ForeignKey(
                        name: "FK_Cases_Tasks_TaskGuid",
                        column: x => x.TaskGuid,
                        principalTable: "Tasks",
                        principalColumn: "TaskGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cases_TaskGuid",
                table: "Cases",
                column: "TaskGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
