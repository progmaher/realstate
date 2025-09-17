using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePropertyModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_District_City_CityId1",
                table: "District");

            migrationBuilder.DropForeignKey(
                name: "FK_District_Country_CountryId",
                table: "District");

            migrationBuilder.DropForeignKey(
                name: "FK_District_Country_CountryId1",
                table: "District");

            migrationBuilder.DropIndex(
                name: "IX_District_CityId1",
                table: "District");

            migrationBuilder.DropIndex(
                name: "IX_District_CountryId1",
                table: "District");

            migrationBuilder.DropColumn(
                name: "CityId1",
                table: "District");

            migrationBuilder.DropColumn(
                name: "CountryId1",
                table: "District");

            migrationBuilder.AddForeignKey(
                name: "FK_District_Country_CountryId",
                table: "District",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_District_Country_CountryId",
                table: "District");

            migrationBuilder.AddColumn<int>(
                name: "CityId1",
                table: "District",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId1",
                table: "District",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_District_CityId1",
                table: "District",
                column: "CityId1");

            migrationBuilder.CreateIndex(
                name: "IX_District_CountryId1",
                table: "District",
                column: "CountryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_District_City_CityId1",
                table: "District",
                column: "CityId1",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_District_Country_CountryId",
                table: "District",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_District_Country_CountryId1",
                table: "District",
                column: "CountryId1",
                principalTable: "Country",
                principalColumn: "Id");
        }
    }
}
