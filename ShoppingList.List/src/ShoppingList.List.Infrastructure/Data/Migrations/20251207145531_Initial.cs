using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShoppingList.List.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:batch_status", "pending,processing,completed,failed")
                .Annotation("Npgsql:Enum:change_type", "create,update,delete")
                .Annotation("Npgsql:Enum:share_permission_type", "read,write");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "Contributors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber_CountryCode = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber_Number = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber_Extension = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shopping_lists",
                columns: table => new
                {
                    list_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping_lists", x => x.list_id);
                });

            migrationBuilder.CreateTable(
                name: "batch_operations",
                columns: table => new
                {
                    batch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    list_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    operations = table.Column<string>(type: "jsonb", nullable: false),
                    status = table.Column<int>(type: "batch_status", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    error_message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batch_operations", x => x.batch_id);
                    table.ForeignKey(
                        name: "FK_batch_operations_shopping_lists_list_id",
                        column: x => x.list_id,
                        principalTable: "shopping_lists",
                        principalColumn: "list_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "list_changes",
                columns: table => new
                {
                    change_id = table.Column<Guid>(type: "uuid", nullable: false),
                    list_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: true),
                    change_type = table.Column<int>(type: "change_type", nullable: false),
                    field_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    old_value = table.Column<string>(type: "text", nullable: true),
                    new_value = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_list_changes", x => x.change_id);
                    table.ForeignKey(
                        name: "FK_list_changes_shopping_lists_list_id",
                        column: x => x.list_id,
                        principalTable: "shopping_lists",
                        principalColumn: "list_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "list_items",
                columns: table => new
                {
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    list_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    category_id = table.Column<Guid>(type: "uuid", nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    is_checked = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_list_items", x => x.item_id);
                    table.ForeignKey(
                        name: "FK_list_items_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_list_items_shopping_lists_list_id",
                        column: x => x.list_id,
                        principalTable: "shopping_lists",
                        principalColumn: "list_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "share_links",
                columns: table => new
                {
                    share_id = table.Column<Guid>(type: "uuid", nullable: false),
                    list_id = table.Column<Guid>(type: "uuid", nullable: false),
                    share_token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    share_permission_type = table.Column<int>(type: "share_permission_type", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_share_links", x => x.share_id);
                    table.ForeignKey(
                        name: "FK_share_links_shopping_lists_list_id",
                        column: x => x.list_id,
                        principalTable: "shopping_lists",
                        principalColumn: "list_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_batch_operations_list_id",
                table: "batch_operations",
                column: "list_id");

            migrationBuilder.CreateIndex(
                name: "IX_list_changes_list_id",
                table: "list_changes",
                column: "list_id");

            migrationBuilder.CreateIndex(
                name: "IX_list_items_category_id",
                table: "list_items",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_list_items_list_id",
                table: "list_items",
                column: "list_id");

            migrationBuilder.CreateIndex(
                name: "IX_share_links_list_id",
                table: "share_links",
                column: "list_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "batch_operations");

            migrationBuilder.DropTable(
                name: "Contributors");

            migrationBuilder.DropTable(
                name: "list_changes");

            migrationBuilder.DropTable(
                name: "list_items");

            migrationBuilder.DropTable(
                name: "share_links");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "shopping_lists");
        }
    }
}
