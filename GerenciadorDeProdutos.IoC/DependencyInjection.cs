using Microsoft.Extensions.DependencyInjection;
using GerenciadorDeProdutos.Domain.Interfaces;
using GerenciadorDeProdutos.Application.Interfaces;
using GerenciadorDeProdutos.Application.Services;
using GerenciadorDeProdutos.Infra.Context;
using GerenciadorDeProdutos.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeProdutos.Infra.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoService, ProdutoService>();

        return services;
    }
}
