using Orderly.Application.Exceptions;
using Orderly.Application.Interfaces;
using Orderly.Application.UseCases.Pedidos.Dto;
using Orderly.Domain.Entities;

namespace Orderly.Application.UseCases.Pedidos
{
    public class ConsultarPedidoUseCase(IPedidoRepository pedidoRepository)
    {
        public async Task<ConsultaPedidoDto> ExecutarAsync(Guid pedidoId,
            CancellationToken cancellationToken = default)
        {
            var pedido = await pedidoRepository.BuscarPorIdAsync(pedidoId, cancellationToken);

            if (pedido == null)
                throw new ExceptionPedidoApplication("Pedido indisponivel");

            List<ConsultaItensDto> consultaItensDto = new();

            var produtosAgrupados = Pedido.AgruparProdutos(pedido.Produtos);
            
            foreach (var itens in produtosAgrupados)
            {
                ConsultaItensDto listaDeItem = new ConsultaItensDto(itens.Produto.Nome, itens.Quantidade, itens.Produto.Preco);
                
                consultaItensDto.Add(listaDeItem);
            }
            
            return new ConsultaPedidoDto(pedido.Id, pedido.Status, pedido.ValorTotal, pedido.DataPedido, consultaItensDto);
        }
    }
}