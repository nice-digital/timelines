using Microsoft.EntityFrameworkCore.Migrations;

namespace NICE.Timelines.DB.Migrations
{
    public partial class AddBooleanFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TaskTypeId",
                table: "TimelineTask",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderInPhase",
                table: "TimelineTask",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClickUpListName",
                table: "TimelineTask",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "KeyDate",
                table: "TimelineTask",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "KeyInfo",
                table: "TimelineTask",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MasterSchedule",
                table: "TimelineTask",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TaskName",
                table: "TimelineTask",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClickUpListName",
                table: "TimelineTask");

            migrationBuilder.DropColumn(
                name: "KeyDate",
                table: "TimelineTask");

            migrationBuilder.DropColumn(
                name: "KeyInfo",
                table: "TimelineTask");

            migrationBuilder.DropColumn(
                name: "MasterSchedule",
                table: "TimelineTask");

            migrationBuilder.DropColumn(
                name: "TaskName",
                table: "TimelineTask");

            migrationBuilder.AlterColumn<int>(
                name: "TaskTypeId",
                table: "TimelineTask",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OrderInPhase",
                table: "TimelineTask",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
