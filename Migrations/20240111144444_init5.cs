using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace istore_api.Migrations
{
    /// <inheritdoc />
    public partial class init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductCharacteristic");

            migrationBuilder.DropColumn(
                name: "Values",
                table: "ProductCharacteristics");

            migrationBuilder.CreateTable(
                name: "ProductCharacteristicVariants",
                columns: table => new
                {
                    CharacteristicName = table.Column<string>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Values = table.Column<string>(type: "TEXT", nullable: false),
                    GrowthToValues = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCharacteristicVariants", x => new { x.ProductId, x.CharacteristicName });
                    table.ForeignKey(
                        name: "FK_ProductCharacteristicVariants_ProductCharacteristics_CharacteristicName",
                        column: x => x.CharacteristicName,
                        principalTable: "ProductCharacteristics",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCharacteristicVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCharacteristicVariants_CharacteristicName",
                table: "ProductCharacteristicVariants",
                column: "CharacteristicName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCharacteristicVariants");

            migrationBuilder.AddColumn<string>(
                name: "Values",
                table: "ProductCharacteristics",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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
    }
}
