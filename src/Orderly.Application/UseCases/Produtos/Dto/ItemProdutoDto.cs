namespace Orderly.Application.UseCases.Produtos.Dto
{
    public record ItemProdutoDto(Guid Id,  string Nome, decimal Preco, int Estoque);
}