using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Produto>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Nome).HasMaxLength(50).IsRequired();
            e.HasIndex(p => p.Nome).IsUnique();
            e.Property(p => p.Preco).IsRequired().HasColumnType("decimal(10,2)");
            e.Property(p => p.Categoria).IsRequired().HasConversion<string>();
        });

        builder.Entity<Pedido>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasMany(p => p.Items).WithMany().UsingEntity(j => j.ToTable("PedidoProdutos"));
            e.Property(p => p.SubTotal).IsRequired().HasColumnType("decimal(10,2)");
            e.Property(p => p.Total).IsRequired().HasColumnType("decimal(10,2)");
        });
        
        // Seeding 
        builder.Entity<Produto>().HasData(
            new Produto { Id = 1, Nome = "X Burger", Preco = 5.00m, Categoria = CategoriaProduto.Sanduiche },
            new Produto { Id = 2, Nome = "X Egg", Preco = 4.50m, Categoria = CategoriaProduto.Sanduiche },
            new Produto { Id = 3, Nome = "X Bacon", Preco = 7.00m, Categoria = CategoriaProduto.Sanduiche },
            new Produto { Id = 4, Nome = "Batata frita", Preco = 2.00m, Categoria = CategoriaProduto.BatataFrita },
            new Produto { Id = 5, Nome = "Refrigerante", Preco = 2.50m, Categoria = CategoriaProduto.Refrigerante }
        );
    }
}