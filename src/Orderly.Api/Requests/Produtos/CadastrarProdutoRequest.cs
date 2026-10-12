namespace Orderly.Api.Requests;

public record CadastrarProdutoRequest(
    string Nome, 
    decimal Preco, 
    int Estoque);