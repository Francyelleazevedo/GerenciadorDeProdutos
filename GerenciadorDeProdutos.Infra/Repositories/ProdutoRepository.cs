using Microsoft.EntityFrameworkCore;
using GerenciadorDeProdutos.Domain.Interfaces;
using GerenciadorDeProdutos.Domain.Entities;
using GerenciadorDeProdutos.Infra.Context;
using GerenciadorDeProdutos.Infra.Exceptions;

namespace GerenciadorDeProdutos.Infra.Repositories
{
    public class ProdutoRepository(ApplicationDbContext context) : IProdutoRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Produto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));

            try
            {
                return await _context.Produto
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id)
                    ?? throw new Exception("Produto não encontrado.");
            }
            catch (DbUpdateException ex)
            {
                throw new InfrastructureException("Erro ao buscar o produto no banco de dados.", ex);
            }
        }

        public async Task AddAsync(Produto produto)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto), "O produto não pode ser nulo.");

            try
            {
                await _context.Produto.AddAsync(produto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InfrastructureException("Erro ao adicionar o produto no banco de dados.", ex);
            }
        }

        public async Task UpdateAsync(Produto produto)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto), "O produto não pode ser nulo.");

            try
            {
                _context.Produto.Update(produto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InfrastructureException("Erro de concorrência ao atualizar o produto.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new InfrastructureException("Erro ao atualizar o produto no banco de dados.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));

            try
            {
                var produto = await GetByIdAsync(id) ?? throw new Exception("Produto não encontrado.");
                _context.Produto.Remove(produto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InfrastructureException("Erro ao excluir o produto no banco de dados.", ex);
            }
        }

        public async Task<int> GetTotalCountAsync()
        {
            try
            {
                return await _context.Produto.CountAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InfrastructureException("Erro ao contar o número total de produtos.", ex);
            }
        }

        public async Task<List<Produto>> GetPagedAsync(int skip, int take)
        {
            if (skip < 0)
                throw new ArgumentException("O valor de 'skip' não pode ser negativo.", nameof(skip));
            if (take <= 0)
                throw new ArgumentException("O valor de 'take' deve ser maior que zero.", nameof(take));

            try
            {
                return await _context.Produto
                    .AsNoTracking()
                    .OrderByDescending(p => p.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erro ao obter a lista paginada de produtos.", ex);
            }
        }
    }
}
