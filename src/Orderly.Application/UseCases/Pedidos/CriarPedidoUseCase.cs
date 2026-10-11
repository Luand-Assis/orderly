using Orderly.Application.Exceptions;
using Orderly.Application.Interfaces;
using Orderly.Application.UseCases.Pedidos.Dto;
using Orderly.Domain.Entities;

namespace Orderly.Application.UseCases.Pedidos
{
    public class CriarPedidoUseCase(IPedidoRepository repositoryPedido,
        IProdutoRepository repositoryProduto)
    {
        public async Task<Guid> ExecutarAsync(List<ItemPedidoDto> produtos,
            CancellationToken cancellationToken = default)
        {
            List<Produto> produtosDoPedido = new();
            
            foreach (var item in produtos)
            {
                var produto = await repositoryProduto.BuscarPorIdAsync(item.ProdutoId, cancellationToken);

                if (produto == null)
                    throw new ExceptionPedidoApplication("Produto invalido");

                for (var i = 0; i < item.Quantidade; i++)
                {
                    produtosDoPedido.Add(produto);
                }
            }

            var pedido = Pedido.CriarPedido(produtosDoPedido);

            await repositoryPedido.AdicionarAsync(pedido, cancellationToken);

            return pedido.Id;
        }
    }
}