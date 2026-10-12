namespace Orderly.Api.Requests.Pedidos;

public record ItemPedidoRequest(Guid ProdutoId, int Quantidade);