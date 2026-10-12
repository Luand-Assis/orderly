using Microsoft.AspNetCore.Mvc;
using Orderly.Api.Requests;
using Orderly.Application.UseCases.Produtos;
using Orderly.Domain.Entities;

namespace Orderly.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController(CadastrarProdutoUseCase useCase) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(CadastrarProdutoRequest request, CancellationToken cancellationToken)
        {
            var id = await useCase.ExecutarAsync(request.Nome, request.Preco, request.Estoque);

            return Created($"/produtos/{id}", new { id });
        }
    }
}