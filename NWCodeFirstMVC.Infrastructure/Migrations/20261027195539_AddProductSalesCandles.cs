using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NWCodeFirstMVC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSalesCandles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_sales_candles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<short>(type: "smallint", nullable: false),
                    week_start = table.Column<DateOnly>(type: "date", nullable: false),
                    open = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    high = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    low = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    close = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_sales_candles", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_sales_candles_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_sales_candles_product_id_week_start",
                table: "product_sales_candles",
                columns: new[] { "product_id", "week_start" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_sales_candles");

        }
    }
}
