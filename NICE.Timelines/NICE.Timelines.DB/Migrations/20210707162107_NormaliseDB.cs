using Microsoft.EntityFrameworkCore.Migrations;

namespace NICE.Timelines.DB.Migrations
{
    public partial class NormaliseDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageDescription",
                table: "TimelineTask");

            migrationBuilder.DropColumn(
                name: "StepDescription",
                table: "TimelineTask");

            migrationBuilder.CreateTable(
                name: "Stage",
                columns: table => new
                {
                    StageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stage", x => x.StageId);
                });

            migrationBuilder.CreateTable(
                name: "Step",
                columns: table => new
                {
                    StepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Step", x => x.StepId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimelineTask_StageId",
                table: "TimelineTask",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineTask_StepId",
                table: "TimelineTask",
                column: "StepId");

            migrationBuilder.AddForeignKey(
                name: "TimelineTasks_Stage",
                table: "TimelineTask",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "StageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "TimelineTasks_Step",
                table: "TimelineTask",
                column: "StepId",
                principalTable: "Step",
                principalColumn: "StepId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "TimelineTasks_Stage",
                table: "TimelineTask");

            migrationBuilder.DropForeignKey(
                name: "TimelineTasks_Step",
                table: "TimelineTask");

            migrationBuilder.DropTable(
                name: "Stage");

            migrationBuilder.DropTable(
                name: "Step");

            migrationBuilder.DropIndex(
                name: "IX_TimelineTask_StageId",
                table: "TimelineTask");

            migrationBuilder.DropIndex(
                name: "IX_TimelineTask_StepId",
                table: "TimelineTask");

            migrationBuilder.AddColumn<string>(
                name: "StageDescription",
                table: "TimelineTask",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StepDescription",
                table: "TimelineTask",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
