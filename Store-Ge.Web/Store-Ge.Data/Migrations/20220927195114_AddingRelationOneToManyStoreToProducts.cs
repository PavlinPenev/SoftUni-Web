using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store_Ge.Data.Migrations
{
    public partial class AddingRelationOneToManyStoreToProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_StoreId",
                table: "Product",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Store_StoreId",
                table: "Product",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Store_StoreId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_StoreId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Product");
        }
    }
}
