using Orderly.Application.Interfaces;
using Orderly.Domain.Entities;

namespace Orderly.Application.UseCases.Produtos
{
    public class CadastrarProdutoUseCase(IProdutoRepository repository)
    {
        public async Task<Guid> ExecutarAsync(string nome, decimal preco, int estoque, 
            CancellationToken cancellationToken = default)
        {
            var produto = Produto.CriarProduto(nome, preco, estoque);

            await repository.AdicionarAsync(produto, cancellationToken);

            return produto.Id;
        }
    }
}