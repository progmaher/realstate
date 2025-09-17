using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingPropertyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressDescription",
                table: "Property",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApartmentNumber",
                table: "Property",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Property",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasElevator",
                table: "Property",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasParking",
                table: "Property",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNegotiable",
                table: "Property",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Kitchens",
                table: "Property",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LivingRooms",
                table: "Property",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationDescription",
                table: "Property",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Property",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThreeDTour",
                table: "Property",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleAr",
                table: "Property",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Property",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressDescription",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "HasElevator",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "HasParking",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "IsNegotiable",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "Kitchens",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "LivingRooms",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "LocationDescription",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "ThreeDTour",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "TitleAr",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Property");
        }
    }
}
