using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderingSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriceOrderClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerCustCode",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProductProdCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "ProductProdCode",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustCode",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerCustCode",
                table: "Orders",
                column: "CustomerCustCode",
                principalTable: "Customers",
                principalColumn: "CustCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProductProdCode",
                table: "Orders",
                column: "ProductProdCode",
                principalTable: "Products",
                principalColumn: "ProdCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerCustCode",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProductProdCode",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "ProductProdCode",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustCode",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerCustCode",
                table: "Orders",
                column: "CustomerCustCode",
                principalTable: "Customers",
                principalColumn: "CustCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProductProdCode",
                table: "Orders",
                column: "ProductProdCode",
                principalTable: "Products",
                principalColumn: "ProdCode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
