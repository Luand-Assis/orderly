using Orderly.Domain.Enums;

namespace Orderly.Application.UseCases.Pedidos.Dto;

public record ConsultaPedidoDto(
    Guid Id,
    StatusPedido Status,
    decimal ValorTotal,
    DateTime DataHora,
    List<ConsultaItensDto> Itens);