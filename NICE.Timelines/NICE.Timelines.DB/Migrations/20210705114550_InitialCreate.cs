using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NICE.Timelines.DB.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimelineTask",
                columns: table => new
                {
                    TimelineTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACID = table.Column<int>(type: "int", nullable: false),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    StepDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    StageDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ClickUpSpaceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClickUpFolderId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClickUpListId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClickUpTaskId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineTask", x => x.TimelineTaskId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimelineTask");
        }
    }
}
