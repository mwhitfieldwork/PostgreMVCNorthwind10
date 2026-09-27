using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NWCodeFirstMVC.Infrastructure.Migrations
{
    public partial class AddUserPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<short>(type: "smallint", nullable: false),
                    category_name = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    picture = table.Column<byte[]>(type: "bytea", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "customer_demographics",
                columns: table => new
                {
                    customer_type_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    customer_desc = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_demographics", x => x.customer_type_id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    customer_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    company_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    contact_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    contact_title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    address = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    city = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    region = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    postal_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    country = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    phone = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    fax = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    employee_id = table.Column<short>(type: "smallint", nullable: false),
                    last_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    first_name = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    title_of_courtesy = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: true),
                    hire_date = table.Column<DateOnly>(type: "date", nullable: true),
                    address = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    city = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    region = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    postal_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    country = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    home_phone = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    extension = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    reports_to = table.Column<short>(type: "smallint", nullable: true),
                    photo_path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.employee_id);
                    table.ForeignKey(
                        name: "fk_employees_employees",
                        column: x => x.reports_to,
                        principalTable: "employees",
                        principalColumn: "employee_id");
                });

            migrationBuilder.CreateTable(
                name: "region",
                columns: table => new
                {
                    region_id = table.Column<short>(type: "smallint", nullable: false),
                    region_description = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_region", x => x.region_id);
                });

            migrationBuilder.CreateTable(
                name: "shippers",
                columns: table => new
                {
                    shipper_id = table.Column<short>(type: "smallint", nullable: false),
                    company_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    phone = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shippers", x => x.shipper_id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    supplier_id = table.Column<short>(type: "smallint", nullable: false),
                    company_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    contact_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    contact_title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    address = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    city = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    region = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    postal_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    country = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    phone = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    fax = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    homepage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suppliers", x => x.supplier_id);
                });

            migrationBuilder.CreateTable(
                name: "us_states",
                columns: table => new
                {
                    state_id = table.Column<short>(type: "smallint", nullable: false),
                    state_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    state_abbr = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    state_region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usstates", x => x.state_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    pkid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    admin = table.Column<bool>(type: "boolean", nullable: false),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    occupation = table.Column<string>(type: "text", nullable: false),
                    Picture = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.pkid);
                });

            migrationBuilder.CreateTable(
                name: "customer_customer_demo",
                columns: table => new
                {
                    customer_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    customer_type_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_customer_demo", x => new { x.customer_id, x.customer_type_id });
                    table.ForeignKey(
                        name: "fk_customer_customer_demo_customer_demographics",
                        column: x => x.customer_type_id,
                        principalTable: "customer_demographics",
                        principalColumn: "customer_type_id");
                    table.ForeignKey(
                        name: "fk_customer_customer_demo_customers",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id");
                });

            migrationBuilder.CreateTable(
                name: "territories",
                columns: table => new
                {
                    territory_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    territory_description = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    region_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_territories", x => x.territory_id);
                    table.ForeignKey(
                        name: "fk_territories_region",
                        column: x => x.region_id,
                        principalTable: "region",
                        principalColumn: "region_id");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    order_id = table.Column<short>(type: "smallint", nullable: false),
                    customer_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    employee_id = table.Column<short>(type: "smallint", nullable: true),
                    order_date = table.Column<DateOnly>(type: "date", nullable: false),
                    required_date = table.Column<DateOnly>(type: "date", nullable: true),
                    shipped_date = table.Column<DateOnly>(type: "date", nullable: true),
                    ship_via = table.Column<short>(type: "smallint", nullable: true),
                    freight = table.Column<float>(type: "real", nullable: true),
                    ship_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    ship_address = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    ship_city = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    ship_region = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    ship_postal_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ship_country = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.order_id);
                    table.ForeignKey(
                        name: "fk_orders_customers",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id");
                    table.ForeignKey(
                        name: "fk_orders_employees",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id");
                    table.ForeignKey(
                        name: "fk_orders_shippers",
                        column: x => x.ship_via,
                        principalTable: "shippers",
                        principalColumn: "shipper_id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    product_id = table.Column<short>(type: "smallint", nullable: false),
                    product_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    supplier_id = table.Column<short>(type: "smallint", nullable: true),
                    category_id = table.Column<short>(type: "smallint", nullable: true),
                    quantity_per_unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    unit_price = table.Column<float>(type: "real", nullable: true),
                    units_in_stock = table.Column<short>(type: "smallint", nullable: true),
                    units_on_order = table.Column<short>(type: "smallint", nullable: true),
                    reorder_level = table.Column<short>(type: "smallint", nullable: true),
                    discontinued = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.product_id);
                    table.ForeignKey(
                        name: "fk_products_categories",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id");
                    table.ForeignKey(
                        name: "fk_products_suppliers",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "supplier_id");
                });

            migrationBuilder.CreateTable(
                name: "employee_territories",
                columns: table => new
                {
                    employee_id = table.Column<short>(type: "smallint", nullable: false),
                    territory_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_territories", x => new { x.employee_id, x.territory_id });
                    table.ForeignKey(
                        name: "fk_employee_territories_employees",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id");
                    table.ForeignKey(
                        name: "fk_employee_territories_territories",
                        column: x => x.territory_id,
                        principalTable: "territories",
                        principalColumn: "territory_id");
                });

            migrationBuilder.CreateTable(
                name: "order_details",
                columns: table => new
                {
                    order_id = table.Column<short>(type: "smallint", nullable: false),
                    product_id = table.Column<short>(type: "smallint", nullable: false),
                    unit_price = table.Column<float>(type: "real", nullable: false),
                    quantity = table.Column<short>(type: "smallint", nullable: false),
                    discount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_details", x => new { x.order_id, x.product_id });
                    table.ForeignKey(
                        name: "fk_order_details_orders",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id");
                    table.ForeignKey(
                        name: "fk_order_details_products",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_customer_customer_demo_customer_type_id",
                table: "customer_customer_demo",
                column: "customer_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_territories_territory_id",
                table: "employee_territories",
                column: "territory_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_reports_to",
                table: "employees",
                column: "reports_to");

            migrationBuilder.CreateIndex(
                name: "IX_order_details_product_id",
                table: "order_details",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_customer_id",
                table: "orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_employee_id",
                table: "orders",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_ship_via",
                table: "orders",
                column: "ship_via");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_supplier_id",
                table: "products",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_territories_region_id",
                table: "territories",
                column: "region_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_customer_demo");

            migrationBuilder.DropTable(
                name: "employee_territories");

            migrationBuilder.DropTable(
                name: "order_details");

            migrationBuilder.DropTable(
                name: "us_states");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "customer_demographics");

            migrationBuilder.DropTable(
                name: "territories");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "region");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "shippers");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "suppliers");
        }
    }
}
