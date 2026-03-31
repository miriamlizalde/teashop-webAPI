using Microsoft.EntityFrameworkCore;
using TeaShop.Models;

namespace TeaShop.Data.Context
{
    public class TeashopContext : DbContext
    {
        public TeashopContext(DbContextOptions<TeashopContext> options) : base(options)
        {
        }
    
            public DbSet<P> Productos { get; set; }
            public DbSet<Pedido> Pedidos { get; set; }
            public DbSet<Usuario> Usuarios { get; set; }

            public DbSet<ItemPedido> ItemPedidos { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Product>()
                    .HasDiscriminator<string>("ProductType")
                    .HasValue<Te>("Tea")
                    .HasValue<Comida>("Food");

                modelBuilder.Entity<Pedido>().HasKey(p => p.Id);
            }
    }        
}