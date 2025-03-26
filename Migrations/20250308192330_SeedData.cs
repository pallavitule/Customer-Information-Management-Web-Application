using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVCDHProject5.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Continent",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Custid", "Balance", "City", "Continent", "Country", "Name", "State", "Status" },
                values: new object[,]
                {
                    { 101, 50000.00m, "Delhi", "North America", "USA", "Sai", "New York", true },
                    { 102, 40000.00m, "Mumbai", "North America", "USA", "Sonia", "New York", true },
                    { 103, 30000.00m, "Chennai", "North America", "USA", "Pankaj", "New York", true },
                    { 104, 25000.00m, "Bengaluru", "North America", "USA", "Samuels", "New York", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Custid",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Custid",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Custid",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Custid",
                keyValue: 104);

            migrationBuilder.DropColumn(
                name: "Continent",
                table: "Customers");
        }
    }
}
