using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddHashToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_ShopProducts_ProductInShopId",
                table: "Prices");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopProducts_Products_ProductId",
                table: "ShopProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopProducts_Shops_ShopId",
                table: "ShopProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopProducts",
                table: "ShopProducts");

            migrationBuilder.RenameTable(
                name: "ShopProducts",
                newName: "ProductsInShops");

            migrationBuilder.RenameIndex(
                name: "IX_ShopProducts_ShopId",
                table: "ProductsInShops",
                newName: "IX_ProductsInShops_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopProducts_ProductId_ShopId",
                table: "ProductsInShops",
                newName: "IX_ProductsInShops_ProductId_ShopId");

            migrationBuilder.AddColumn<int>(
                name: "Hash",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsInShops",
                table: "ProductsInShops",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_ProductsInShops_ProductInShopId",
                table: "Prices",
                column: "ProductInShopId",
                principalTable: "ProductsInShops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsInShops_Products_ProductId",
                table: "ProductsInShops",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsInShops_Shops_ShopId",
                table: "ProductsInShops",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_ProductsInShops_ProductInShopId",
                table: "Prices");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsInShops_Products_ProductId",
                table: "ProductsInShops");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsInShops_Shops_ShopId",
                table: "ProductsInShops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsInShops",
                table: "ProductsInShops");

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "ProductsInShops",
                newName: "ShopProducts");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsInShops_ShopId",
                table: "ShopProducts",
                newName: "IX_ShopProducts_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsInShops_ProductId_ShopId",
                table: "ShopProducts",
                newName: "IX_ShopProducts_ProductId_ShopId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopProducts",
                table: "ShopProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_ShopProducts_ProductInShopId",
                table: "Prices",
                column: "ProductInShopId",
                principalTable: "ShopProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProducts_Products_ProductId",
                table: "ShopProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProducts_Shops_ShopId",
                table: "ShopProducts",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
