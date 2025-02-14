using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeProdutos.Web.Models
{
    public class ProdutoViewModel
    {

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        public string Preco { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória.")]
        public int QuantidadeEstoque { get; set; }
    }
}
