using Microsoft.EntityFrameworkCore.Migrations;

namespace GeoCam.Api.Migrations.Migrations
{
    public partial class CameraTableAddNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Cameras",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Cameras");
        }
    }
}
