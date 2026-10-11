using JetBrains.Annotations;
using Moq;
using Orderly.Application.Interfaces;
using Orderly.Application.UseCases.Pedidos;
using Orderly.Application.UseCases.Pedidos.Dto;
using Orderly.Domain.Entities;

namespace Orderly.Tests.UseCases.Pedidos;

[TestSubject(typeof(CriarPedidoUseCase))]
public class CriarPedidoUseCaseTest
{

    [Fact]
    public async Task Executar_ComDadosValidos_DeveSalvarComandaEBaixarEstoque()
    {
        var produto1 = Produto.CriarProduto("Produto 1", 100m, 10);
        var produto2 = Produto.CriarProduto("Produto 2", 200m, 5);

        var produtoRepository = new Mock<IProdutoRepository>();

        produtoRepository.Setup(r => 
                r.BuscarPorIdAsync(produto1.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produto1);

        produtoRepository.Setup(r => 
                r.BuscarPorIdAsync(produto2.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produto2);
        
        var pedidoRepository = new Mock<IPedidoRepository>();
        var useCase = new CriarPedidoUseCase(pedidoRepository.Object, produtoRepository.Object);

        var itens = new List<ItemPedidoDto>
        {
            new(produto1.Id, 5),
            new(produto2.Id, 2)
        }; 
        
        await useCase.ExecutarAsync(itens);
        
        pedidoRepository.Verify(r =>
            r.AdicionarAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()));
        
        Assert.Equal(5, produto1.Estoque);
        Assert.Equal(3, produto2.Estoque);
    }
}