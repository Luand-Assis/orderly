namespace Orderly.Application.UseCases.Pedidos.Dto;

public record ConsultaItensDto(
    string Nome,
    int Quantidade,
    decimal PrecoUnitario);