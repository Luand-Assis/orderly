using Orderly.Application.Exceptions;
using Orderly.Application.Interfaces;

namespace Orderly.Application.UseCases.Pedidos
{
    public class PagarPedidoUseCase(IPedidoRepository pedidoRepository)
    {
        public async Task ExecutarAsync(Guid pedidoId,
            CancellationToken cancellationToken = default)
        {
            var item = await pedidoRepository.BuscarPorIdAsync(pedidoId, cancellationToken);

            if (item == null)
                throw new ExceptionPedidoApplication("Pedido indisponivel");

            item.Pagar();

            await pedidoRepository.AtualizarAsync(item, cancellationToken);
        }
    }
}