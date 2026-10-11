using Orderly.Domain.Entities;

namespace Orderly.Application.Interfaces
{
    public interface IProdutoRepository
    {
        Task AdicionarAsync(Produto produto, CancellationToken cancellationToken = default);
        
        Task<Produto?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}