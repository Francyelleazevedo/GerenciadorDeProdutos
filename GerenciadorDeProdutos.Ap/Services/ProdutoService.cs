using GerenciadorDeProdutos.Application.Common;
using GerenciadorDeProdutos.Application.Interfaces;
using GerenciadorDeProdutos.Domain.Interfaces;
using GerenciadorDeProdutos.Domain.Entities;

namespace GerenciadorDeProdutos.Application.Services
{
    public class ProdutoService(IProdutoRepository produtoRepository) : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        public async Task AddAsync(Produto produto)
        {
            if (produto == null) throw new ArgumentNullException(nameof(produto), "O produto não pode ser nulo.");
            try
            {
                if(produto.QuantidadeEstoque < 0)
                {
                    throw new Exception("Não pode inserir produtos com a quantidade de estoque menor que 0.");
                }
                await _produtoRepository.AddAsync(produto);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar o produto.", ex);
            }
        }

        public async Task<PagedResult<Produto>> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) throw new ArgumentException("O número da página deve ser maior que zero.", nameof(pageNumber));
            if (pageSize <= 0) throw new ArgumentException("O tamanho da página deve ser maior que zero.", nameof(pageSize));
            try
            {
                int totalItems = await _produtoRepository.GetTotalCountAsync();
                int skip = (pageNumber - 1) * pageSize;
                var produtos = await _produtoRepository.GetPagedAsync(skip, pageSize);

                return new PagedResult<Produto>
                {
                    Items = produtos,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter a lista de produtos.", ex);
            }
        }

        public async Task<Produto> GetByIdAsync(int id)
        {
            ValidateId(id);
            try
            {
                var produto = await _produtoRepository.GetByIdAsync(id);
                return produto ?? throw new Exception("Produto não encontrado.");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter o produto.", ex);
            }
        }

        public async Task UpdateAsync(Produto produto)
        {
            if (produto == null) throw new ArgumentNullException(nameof(produto), "O produto não pode ser nulo.");
            try
            {
                if (produto.QuantidadeEstoque < 0)
                {
                    throw new Exception("Não pode inserir produtos com a quantidade de estoque menor que 0.");
                }
                await _produtoRepository.UpdateAsync(produto);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o produto.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            ValidateId(id);
            try
            {
                var produto = await _produtoRepository.GetByIdAsync(id) ?? throw new Exception("Produto não encontrado.");

                if (produto.QuantidadeEstoque == 0)
                {
                    await _produtoRepository.DeleteAsync(id);
                }
                else
                {
                    throw new Exception("O produto só pode ser removido se sua quantidade em estoque for zero.");
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir o produto.", ex);
            }
        }

        private static void ValidateId(int id)
        {
            if (id <= 0) throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        }
    }
}
