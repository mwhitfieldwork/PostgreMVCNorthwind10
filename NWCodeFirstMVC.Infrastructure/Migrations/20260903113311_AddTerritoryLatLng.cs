using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NWCodeFirstMVC.Infrastructure.Migrations
{
    public partial class AddTerritoryLatLng : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "users",
                newName: "picture");

            migrationBuilder.AlterColumn<string>(
                name: "occupation",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<decimal>(
                name: "latitude",
                table: "territories",
                type: "numeric(9,6)",
                precision: 9,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "longitude",
                table: "territories",
                type: "numeric(9,6)",
                precision: 9,
                scale: 6,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                table: "territories");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "territories");

            migrationBuilder.RenameColumn(
                name: "picture",
                table: "users",
                newName: "Picture");

            migrationBuilder.AlterColumn<string>(
                name: "occupation",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
