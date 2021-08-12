using Microsoft.EntityFrameworkCore.Migrations;

namespace NICE.Timelines.DB.Migrations
{
    public partial class AddColumnOrderInPhase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderInPhase",
                table: "TimelineTask",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderInPhase",
                table: "TimelineTask");
        }
    }
}
