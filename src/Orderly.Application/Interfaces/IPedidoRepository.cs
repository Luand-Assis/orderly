using Orderly.Domain.Entities;

namespace Orderly.Application.Interfaces;

public interface IPedidoRepository
{
    Task AdicionarAsync(Pedido pedido, CancellationToken cancellationToken = default);
        
    Task<Pedido?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task AtualizarAsync(Pedido pedido, CancellationToken cancellationToken = default);
}