using GerenciadorDeProdutos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeProdutos.Infra.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Produto> Produto { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
