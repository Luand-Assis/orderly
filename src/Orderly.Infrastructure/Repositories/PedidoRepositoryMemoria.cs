using Orderly.Application.Interfaces;
using Orderly.Domain.Entities;

namespace Orderly.Infrastructure.Repositories
{
    public class PedidoRepositoryMemoria : IPedidoRepository
    {
        private readonly Dictionary<Guid, Pedido> _pedidos = new();

        public Task AdicionarAsync(Pedido pedido, CancellationToken cancellationToken = default)
        {
            _pedidos[pedido.Id] = pedido;
            return Task.CompletedTask;
        }

        public Task<Pedido?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _pedidos.TryGetValue(id, out var pedido);
            return Task.FromResult(pedido);
        }
    }
}