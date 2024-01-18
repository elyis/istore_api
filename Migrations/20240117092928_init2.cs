using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace istore_api.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCharacteristicVariants");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ProductCharacteristics",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "ProductCharacteristics");

            migrationBuilder.CreateTable(
                name: "ProductCharacteristicVariants",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CharacteristicName = table.Column<string>(type: "TEXT", nullable: false),
                    CharacteristicId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GrowthToValues = table.Column<string>(type: "TEXT", nullable: false),
                    Values = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCharacteristicVariants", x => new { x.ProductId, x.CharacteristicName });
                    table.ForeignKey(
                        name: "FK_ProductCharacteristicVariants_ProductCharacteristics_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalTable: "ProductCharacteristics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCharacteristicVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Filename = table.Column<string>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: false),
                    Hex = table.Column<string>(type: "TEXT", nullable: false),
                    IsPreviewImage = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Filename);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCharacteristicVariants_CharacteristicId",
                table: "ProductCharacteristicVariants",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");
        }
    }
}
