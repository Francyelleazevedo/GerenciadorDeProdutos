using GerenciadorDeProdutos.Application.Common;
using GerenciadorDeProdutos.Domain.Entities;
using System.Threading.Tasks;

namespace GerenciadorDeProdutos.Application.Interfaces
{
    public interface IProdutoService
    {
        Task AddAsync(Produto produto);
        Task<PagedResult<Produto>> GetAllAsync(int pageNumber, int pageSize);
        Task<Produto> GetByIdAsync(int id);
        Task UpdateAsync(Produto produto);
        Task DeleteAsync(int id);
    }
}
