using Orderly.Application.Exceptions;
using Orderly.Application.Interfaces;
using Orderly.Application.UseCases.Produtos.Dto;

namespace Orderly.Application.UseCases.Produtos
{
    public class ConsultarProdutoUseCase(IProdutoRepository produtoRepository)
    {
        public async Task<ItemProdutoDto> ExecutarAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var produto = await produtoRepository.BuscarPorIdAsync(id, cancellationToken);
            
            return produto is null
                ? throw new ExceptionProdutoApplication("Produto invalido")
                : new ItemProdutoDto(produto.Id, produto.Nome, produto.Preco, produto.Estoque);
        }
    }
}