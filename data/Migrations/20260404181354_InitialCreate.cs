using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeaShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    ProductoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Origen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    EsOrganico = table.Column<bool>(type: "bit", nullable: false),
                    FechaCaducidad = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoProducto = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    TipoComida = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gluten = table.Column<bool>(type: "bit", nullable: true),
                    TipoHoja = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.ProductoId);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EsSocio = table.Column<bool>(type: "bit", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    PedidoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrecioTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.PedidoId);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemsPedido",
                columns: table => new
                {
                    ItemPedidoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<double>(type: "float", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsPedido", x => x.ItemPedidoId);
                    table.ForeignKey(
                        name: "FK_ItemsPedido_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "PedidoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemsPedido_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "ProductoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "ProductoId", "EsOrganico", "FechaCaducidad", "Nombre", "Origen", "Precio", "Stock", "TipoHoja", "TipoProducto" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Té Matcha", "Japón", 50.5m, 1000, "Verde", "Te" },
                    { 2, false, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Té Chai", "India", 32.0m, 10000, "Negro", "Te" },
                    { 3, false, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Té Puerh", "China", 22.0m, 10000, "Rojo", "Te" },
                    { 4, true, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Té Oolong", "China", 45.0m, 1500, "Azul", "Te" }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "ProductoId", "EsOrganico", "FechaCaducidad", "Gluten", "Nombre", "Origen", "Precio", "Stock", "TipoComida", "TipoProducto" },
                values: new object[,]
                {
                    { 5, false, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Tarta de Pistacho", "España", 25.0m, 5000, "Dulce", "Comida" },
                    { 6, true, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cookie de Avena", "España", 17.8m, 2500, "Dulce", "Comida" },
                    { 7, false, new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Sándwich Vegetal", "Francia", 23.5m, 2200, "Salado", "Comida" },
                    { 8, true, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Wrap de Pollo", "Reino Unido", 20.2m, 6400, "Salado", "Comida" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemsPedido_PedidoId",
                table: "ItemsPedido",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsPedido_ProductoId",
                table: "ItemsPedido",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemsPedido");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
