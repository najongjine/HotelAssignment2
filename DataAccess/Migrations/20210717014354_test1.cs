using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "HotelAmenities",
                newName: "Test1");

            migrationBuilder.AddColumn<string>(
                name: "Name1",
                table: "HotelAmenities",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name1",
                table: "HotelAmenities");

            migrationBuilder.RenameColumn(
                name: "Test1",
                table: "HotelAmenities",
                newName: "Name");
        }
    }
}
