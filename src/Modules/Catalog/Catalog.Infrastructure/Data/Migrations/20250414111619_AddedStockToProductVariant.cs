using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arzand.Modules.Catalog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedStockToProductVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "ProductVariants",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "ProductVariants");
        }
    }
}
