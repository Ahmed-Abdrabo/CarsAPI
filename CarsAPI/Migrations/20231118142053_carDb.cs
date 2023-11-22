using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarsAPI.Migrations
{
    /// <inheritdoc />
    public partial class carDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    VIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransmissionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mileage = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FuelEfficiency = table.Column<double>(type: "float", nullable: false),
                    BodyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "BodyType", "Color", "Condition", "EngineType", "FuelEfficiency", "ImageUrl", "Make", "Mileage", "Model", "Price", "TransmissionType", "VIN", "Year" },
                values: new object[,]
                {
                    { 1, "Sedan", "Silver", "Used", "Gasoline", 28.5, "https://example.com/toyota_camry_image.jpg", "Toyota", 25000.0, "Camry", 20000m, "Automatic", "1HGCM82633A004352", 2020 },
                    { 2, "SUV", "Blue", "Used", "Gasoline", 30.199999999999999, "https://example.com/honda_crv_image.jpg", "Honda", 30000.0, "CR-V", 23000m, "CVT", "5J6RW1H58HL000568", 2018 },
                    { 3, "Coupe", "Red", "New", "Gasoline", 25.0, "https://example.com/ford_mustang_image.jpg", "Ford", 10000.0, "Mustang", 35000m, "Manual", "1FA6P8TH6M5107921", 2022 },
                    { 4, "SUV", "Black", "Used", "Gasoline", 18.600000000000001, "https://example.com/chevrolet_tahoe_image.jpg", "Chevrolet", 15000.0, "Tahoe", 45000m, "Automatic", "1GNSKCKC3MR379084", 2021 },
                    { 5, "SUV", "White", "Used", "Gasoline", 24.5, "https://example.com/bmw_x5_image.jpg", "BMW", 20000.0, "X5", 50000m, "Automatic", "5UXCR6C56KLL31682", 2019 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
