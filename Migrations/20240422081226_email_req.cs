using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace istore_api.Migrations
{
    /// <inheritdoc />
    public partial class email_req : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "InitialRegistrations",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "InitialRegistrations",
                newName: "Phone");
        }
    }
}
