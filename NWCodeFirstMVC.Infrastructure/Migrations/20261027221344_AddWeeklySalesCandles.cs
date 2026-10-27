using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NWCodeFirstMVC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWeeklySalesCandles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "weekly_sales_candles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    week_start = table.Column<DateOnly>(type: "date", nullable: false),
                    open = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    high = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    low = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    close = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weekly_sales_candles", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_weekly_sales_candles_week_start",
                table: "weekly_sales_candles",
                column: "week_start",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weekly_sales_candles");
        }
    }
}
