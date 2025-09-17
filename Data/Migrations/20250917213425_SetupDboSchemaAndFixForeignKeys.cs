using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetupDboSchemaAndFixForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_District_City_CityId",
                table: "District");

            migrationBuilder.DropForeignKey(
                name: "FK_District_Country_CountryId",
                table: "District");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "PropertyImage",
                newName: "PropertyImage",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Property",
                newName: "Property",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "District",
                newName: "District",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Country",
                newName: "Country",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "City",
                newName: "City",
                newSchema: "dbo");

            migrationBuilder.AddColumn<int>(
                name: "CityId1",
                schema: "dbo",
                table: "District",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId1",
                schema: "dbo",
                table: "District",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_District_CityId1",
                schema: "dbo",
                table: "District",
                column: "CityId1");

            migrationBuilder.CreateIndex(
                name: "IX_District_CountryId1",
                schema: "dbo",
                table: "District",
                column: "CountryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_District_City_CityId",
                schema: "dbo",
                table: "District",
                column: "CityId",
                principalSchema: "dbo",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_District_City_CityId1",
                schema: "dbo",
                table: "District",
                column: "CityId1",
                principalSchema: "dbo",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_District_Country_CountryId",
                schema: "dbo",
                table: "District",
                column: "CountryId",
                principalSchema: "dbo",
                principalTable: "Country",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_District_Country_CountryId1",
                schema: "dbo",
                table: "District",
                column: "CountryId1",
                principalSchema: "dbo",
                principalTable: "Country",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_District_City_CityId",
                schema: "dbo",
                table: "District");

            migrationBuilder.DropForeignKey(
                name: "FK_District_City_CityId1",
                schema: "dbo",
                table: "District");

            migrationBuilder.DropForeignKey(
                name: "FK_District_Country_CountryId",
                schema: "dbo",
                table: "District");

            migrationBuilder.DropForeignKey(
                name: "FK_District_Country_CountryId1",
                schema: "dbo",
                table: "District");

            migrationBuilder.DropIndex(
                name: "IX_District_CityId1",
                schema: "dbo",
                table: "District");

            migrationBuilder.DropIndex(
                name: "IX_District_CountryId1",
                schema: "dbo",
                table: "District");

            migrationBuilder.DropColumn(
                name: "CityId1",
                schema: "dbo",
                table: "District");

            migrationBuilder.DropColumn(
                name: "CountryId1",
                schema: "dbo",
                table: "District");

            migrationBuilder.RenameTable(
                name: "PropertyImage",
                schema: "dbo",
                newName: "PropertyImage");

            migrationBuilder.RenameTable(
                name: "Property",
                schema: "dbo",
                newName: "Property");

            migrationBuilder.RenameTable(
                name: "District",
                schema: "dbo",
                newName: "District");

            migrationBuilder.RenameTable(
                name: "Country",
                schema: "dbo",
                newName: "Country");

            migrationBuilder.RenameTable(
                name: "City",
                schema: "dbo",
                newName: "City");

            migrationBuilder.AddForeignKey(
                name: "FK_District_City_CityId",
                table: "District",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_District_Country_CountryId",
                table: "District",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
