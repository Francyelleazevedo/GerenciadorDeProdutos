using GerenciadorDeProdutos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciadorDeProdutos.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<int> GetTotalCountAsync();
        Task<List<Produto>> GetPagedAsync(int skip, int take);
        Task<Produto> GetByIdAsync(int id);
        Task AddAsync(Produto produto);
        Task UpdateAsync(Produto produto);
        Task DeleteAsync(int id);
    }
}
