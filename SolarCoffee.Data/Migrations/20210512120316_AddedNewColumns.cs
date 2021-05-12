using Microsoft.EntityFrameworkCore.Migrations;

namespace SolarCoffee.Data.Migrations
{
    public partial class AddedNewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Qunatity",
                table: "GetSalesOrderItems",
                newName: "Quantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "GetSalesOrderItems",
                newName: "Qunatity");
        }
    }
}
