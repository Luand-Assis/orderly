using Microsoft.AspNetCore.Mvc;
using Orderly.Api.Requests;
using Orderly.Application.UseCases.Produtos;
using Orderly.Application.UseCases.Produtos.Dto;
using Orderly.Domain.Entities;

namespace Orderly.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController(
        CadastrarProdutoUseCase cadastrarProdutoUseCase,
        ConsultarProdutoUseCase consultarProdutoUseCase) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CadastrarProduto(CadastrarProdutoRequest request, CancellationToken cancellationToken)
        {
            var id = await cadastrarProdutoUseCase.ExecutarAsync(request.Nome, request.Preco, request.Estoque);

            return Created($"/produtos/{id}", new { id });
        }

        [HttpGet("{produtoId:guid}")]
        public async Task<ActionResult<ItemProdutoDto>> ConsultarProduto(Guid produtoId, CancellationToken cancellationToken)
        {
            var produto = await consultarProdutoUseCase.ExecutarAsync(produtoId, cancellationToken);

            return Ok(produto);
        }
    }
}