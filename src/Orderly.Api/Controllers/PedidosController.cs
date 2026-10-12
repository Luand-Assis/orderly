using Microsoft.AspNetCore.Mvc;
using Orderly.Api.Requests.Pedidos;
using Orderly.Application.UseCases.Pedidos;
using Orderly.Application.UseCases.Pedidos.Dto;
using Orderly.Domain.Entities;

namespace Orderly.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidosController(
        CriarPedidoUseCase criarPedidoUseCase,
        PagarPedidoUseCase pagarPedidoUseCase,
        CancelarPedidoUseCase cancelarPedidoUseCase,
        ConsultarPedidoUseCase consultarPedidoUseCase)
        : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPedidoRequest request, CancellationToken cancellationToken)
        {
            var itens = request.Itens
                .Select(i => new ItemPedidoDto(i.ProdutoId, i.Quantidade))
                .ToList();

            var id = await criarPedidoUseCase.ExecutarAsync(itens, cancellationToken);

            return Created($"/pedidos/{id}", new { id });
        }
        
        [HttpGet("{pedidoId:guid}")]
        public async Task<ActionResult<ConsultaPedidoDto>> Consultar(Guid pedidoId, CancellationToken cancellationToken)
        {
            var pedido = await consultarPedidoUseCase.ExecutarAsync(pedidoId, cancellationToken);

            return Ok(pedido);
        }

        [HttpPost("{pedidoId:guid}/pagar")]
        public async Task<IActionResult> Pagar(Guid pedidoId, CancellationToken cancellationToken)
        {
            await pagarPedidoUseCase.ExecutarAsync(pedidoId, cancellationToken);

            return Ok();
        }

        [HttpPost("{pedidoId:guid}/cancelar")]
        public async Task<IActionResult> Cancelar(Guid pedidoId, CancellationToken cancellationToken)
        {
            await cancelarPedidoUseCase.ExecutarAsync(pedidoId, cancellationToken);

            return Ok();
        }
    }
}