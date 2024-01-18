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
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ProductConfigCharacteristics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ProductConfigCharacteristics",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
