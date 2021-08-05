using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NICE.Timelines.DB.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Phase",
                columns: table => new
                {
                    PhaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhaseDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phase", x => x.PhaseId);
                });

            migrationBuilder.CreateTable(
                name: "TimelineTask",
                columns: table => new
                {
                    TimelineTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACID = table.Column<int>(type: "int", nullable: false),
                    TaskTypeId = table.Column<int>(type: "int", nullable: true),
                    PhaseId = table.Column<int>(type: "int", nullable: false),
                    ClickUpSpaceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClickUpFolderId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClickUpListId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClickUpTaskId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineTask", x => x.TimelineTaskId);
                    table.ForeignKey(
                        name: "TimelineTasks_Stage",
                        column: x => x.PhaseId,
                        principalTable: "Phase",
                        principalColumn: "PhaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Phase",
                columns: new[] { "PhaseId", "PhaseDescription" },
                values: new object[,]
                {
                    { 12, "Invitation to participate" },
                    { 28, "Consultee meeting" },
                    { 27, "Scope review" },
                    { 26, "Committee meeting" },
                    { 24, "Publication" },
                    { 22, "FAD appeal period" },
                    { 21, "FAD sign off" },
                    { 109, "Scoping" },
                    { 20, "Consultation" },
                    { 18, "Pre meeting briefing" },
                    { 17, "Evidence critique" },
                    { 16, "Overview" },
                    { 15, "Assessment report consultation" },
                    { 14, "Assessment report" },
                    { 13, "Submissions" },
                    { 19, "First committee meeting" },
                    { 113, "Technical report" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimelineTask_PhaseId",
                table: "TimelineTask",
                column: "PhaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimelineTask");

            migrationBuilder.DropTable(
                name: "Phase");
        }
    }
}
