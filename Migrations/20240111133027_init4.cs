using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace istore_api.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCharacteristics_Products_ProductId",
                table: "ProductCharacteristics");

            migrationBuilder.DropIndex(
                name: "IX_ProductCharacteristics_ProductId",
                table: "ProductCharacteristics");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductCharacteristics");

            migrationBuilder.CreateTable(
                name: "ProductProductCharacteristic",
                columns: table => new
                {
                    CharacteristicsName = table.Column<string>(type: "TEXT", nullable: false),
                    ProductsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductCharacteristic", x => new { x.CharacteristicsName, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ProductProductCharacteristic_ProductCharacteristics_CharacteristicsName",
                        column: x => x.CharacteristicsName,
                        principalTable: "ProductCharacteristics",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductCharacteristic_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductCharacteristic_ProductsId",
                table: "ProductProductCharacteristic",
                column: "ProductsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductCharacteristic");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ProductCharacteristics",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductCharacteristics_ProductId",
                table: "ProductCharacteristics",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCharacteristics_Products_ProductId",
                table: "ProductCharacteristics",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
