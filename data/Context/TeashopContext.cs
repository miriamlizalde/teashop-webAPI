using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeaShop.Models;

namespace TeaShop.Data
{
    public class TeashopContext : DbContext
    {
        public TeashopContext(DbContextOptions<TeashopContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>()
                .HasDiscriminator<string>("TipoProducto")
                .HasValue<Te>("Te")
                .HasValue<Comida>("Comida");

            modelBuilder.Entity<Te>().HasData(
            new Te { ProductoId = 1, Nombre = "Té Matcha", Origen = "Japón",
                     Precio = 50.5m, Stock = 1000, EsOrganico = true,
                     TipoHoja = "Verde", FechaCaducidad = new DateTime(2026, 6, 1) },
            new Te { ProductoId = 2, Nombre = "Té Chai", Origen = "India",
                     Precio = 32.0m, Stock = 10000, EsOrganico = false,
                     TipoHoja = "Negro", FechaCaducidad = new DateTime(2026, 8, 1) },
            new Te { ProductoId = 3, Nombre = "Té Puerh", Origen = "China",
                     Precio = 22.0m, Stock = 10000, EsOrganico = false,
                     TipoHoja = "Rojo", FechaCaducidad = new DateTime(2026, 9, 1) },
            new Te { ProductoId = 4, Nombre = "Té Oolong", Origen = "China",
                     Precio = 45.0m, Stock = 1500, EsOrganico = true,
                     TipoHoja = "Azul", FechaCaducidad = new DateTime(2026, 7, 1) }
        );

        modelBuilder.Entity<Comida>().HasData(
            new Comida { ProductoId = 5, Nombre = "Tarta de Pistacho", Origen = "España",
                         Precio = 25.0m, Stock = 5000, EsOrganico = false,
                         TipoComida = "Dulce", Gluten = true,
                         FechaCaducidad = new DateTime(2026, 4, 1) },
            new Comida { ProductoId = 6, Nombre = "Cookie de Avena", Origen = "España",
                         Precio = 17.8m, Stock = 2500, EsOrganico = true,
                         TipoComida = "Dulce", Gluten = false,
                         FechaCaducidad = new DateTime(2026, 5, 1) },
            new Comida { ProductoId = 7, Nombre = "Sándwich Vegetal", Origen = "Francia",
                         Precio = 23.5m, Stock = 2200, EsOrganico = false,
                         TipoComida = "Salado", Gluten = true,
                         FechaCaducidad = new DateTime(2026, 4, 15) },
            new Comida { ProductoId = 8, Nombre = "Wrap de Pollo", Origen = "Reino Unido",
                         Precio = 20.2m, Stock = 6400, EsOrganico = true,
                         TipoComida = "Salado", Gluten = false,
                         FechaCaducidad = new DateTime(2026, 5, 10) }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        {
            optionsBuilder
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }
    }
    
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItemsPedido { get; set; }
    }             
}