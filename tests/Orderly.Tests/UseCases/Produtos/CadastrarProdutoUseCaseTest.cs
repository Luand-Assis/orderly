using JetBrains.Annotations;
using Moq;
using Orderly.Application.Interfaces;
using Orderly.Application.UseCases.Produtos;
using Orderly.Domain.Entities;


namespace Orderly.Tests.UseCases.Produtos;

[TestSubject(typeof(CadastrarProdutoUseCase))]
public class CadastrarProdutoUseCaseTest
{

    [Fact]
    public async Task Executar_ComDadosValidos_DeveSalvarProduto()
    {
        var repository = new Mock<IProdutoRepository>();
        var useCase = new CadastrarProdutoUseCase(repository.Object);

        var id = await useCase.ExecutarAsync("Produto 1", 100m, 10);
        
        repository.Verify(
            r => r.AdicionarAsync(
                It.Is<Produto>(p => p.Id == id && p.Nome == "Produto 1"), 
                It.IsAny<CancellationToken>()));
    }
}