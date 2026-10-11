using Orderly.Application.Interfaces;
using Orderly.Domain.Entities;

namespace Orderly.Infrastructure.Repositories
{
    public class ProdutoRepositoryMemoria : IProdutoRepository
    {
        private readonly Dictionary<Guid, Produto> _produtos = new();

        public Task AdicionarAsync(Produto produto, CancellationToken cancellationToken = default)
        {
            _produtos[produto.Id] = produto;
            return Task.CompletedTask;
        }

        public Task<Produto?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _produtos.TryGetValue(id, out var produto);
            return Task.FromResult(produto);
        }
    }
}