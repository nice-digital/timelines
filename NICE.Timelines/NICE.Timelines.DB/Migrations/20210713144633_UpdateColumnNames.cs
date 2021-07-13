using Microsoft.EntityFrameworkCore.Migrations;

namespace NICE.Timelines.DB.Migrations
{
    public partial class UpdateColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "StepId",
                table: "TimelineTask",
                newName: "TaskTypeId");

            migrationBuilder.RenameColumn(
                name: "StageId",
                table: "TimelineTask",
                newName: "PhaseId");

            migrationBuilder.RenameIndex(
                name: "IX_TimelineTask_StepId",
                table: "TimelineTask",
                newName: "IX_TimelineTask_TaskTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TimelineTask_StageId",
                table: "TimelineTask",
                newName: "IX_TimelineTask_PhaseId");

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
                name: "TaskType",
                columns: table => new
                {
                    TaskTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskType", x => x.TaskTypeId);
                });

            migrationBuilder.AddForeignKey(
                name: "TimelineTasks_Stage",
                table: "TimelineTask",
                column: "PhaseId",
                principalTable: "Phase",
                principalColumn: "PhaseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "TimelineTasks_Step",
                table: "TimelineTask",
                column: "TaskTypeId",
                principalTable: "TaskType",
                principalColumn: "TaskTypeId",
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
                name: "Phase");

            migrationBuilder.DropTable(
                name: "TaskType");

            migrationBuilder.RenameColumn(
                name: "TaskTypeId",
                table: "TimelineTask",
                newName: "StepId");

            migrationBuilder.RenameColumn(
                name: "PhaseId",
                table: "TimelineTask",
                newName: "StageId");

            migrationBuilder.RenameIndex(
                name: "IX_TimelineTask_TaskTypeId",
                table: "TimelineTask",
                newName: "IX_TimelineTask_StepId");

            migrationBuilder.RenameIndex(
                name: "IX_TimelineTask_PhaseId",
                table: "TimelineTask",
                newName: "IX_TimelineTask_StageId");

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
    }
}
