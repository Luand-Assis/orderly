using Orderly.Application.Exceptions;
using Orderly.Application.Interfaces;

namespace Orderly.Application.UseCases.Pedidos
{
    public class CancelarPedidoUseCase(IPedidoRepository pedidoRepository)
    {
        public async Task ExecutarAsync(Guid pedidoId,
            CancellationToken cancellationToken = default)
        {
            var item = await pedidoRepository.BuscarPorIdAsync(pedidoId, cancellationToken);

            if (item == null)
                throw new ExceptionPedidoApplication("Pedido indisponivel");
            
            item.Cancelar();
            
            await pedidoRepository.AtualizarAsync(item, cancellationToken);
        }
    }
}