using JetBrains.Annotations;
using Moq;
using Orderly.Application.Exceptions;
using Orderly.Application.Interfaces;
using Orderly.Application.UseCases.Pedidos;
using Orderly.Domain.Entities;
using Orderly.Domain.Enums;

namespace Orderly.Tests.UseCases.Pedidos;

[TestSubject(typeof(PagarPedidoUseCase))]
public class PagarPedidoUseCaseTest
{
    [Fact]
    public async void Pagar_ComDadosValidos_DevePagarOPedido()
    {
        var produto1 = Produto.CriarProduto("produto 1", 100m, 5);
        var produto2 = Produto.CriarProduto("produto 2", 200m, 3);

        List<Produto> produtos = new List<Produto>() { produto1, produto2 };
        
        var pedido = Pedido.CriarPedido(produtos);
        
        var pedidoRepository = new Mock<IPedidoRepository>();

        pedidoRepository.Setup(r =>
                r.BuscarPorIdAsync(pedido.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedido);

        var useCase = new PagarPedidoUseCase(pedidoRepository.Object);
        
        await useCase.ExecutarAsync(pedido.Id);
        
        pedidoRepository.Verify(r =>
            r.AtualizarAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()));
        
        Assert.Equal(StatusPedido.Processando, pedido.Status);
    }
    
    [Fact]
    public async void Pagar_ComPedidoInexistente_NaoDevePagar()
    {
        var produto1 = Produto.CriarProduto("produto 1", 100m, 5);
        var produto2 = Produto.CriarProduto("produto 2", 200m, 3);

        List<Produto> produtos = new List<Produto>() { produto1, produto2 };
        
        var pedido = Pedido.CriarPedido(produtos);
            
        var pedidoRepository = new Mock<IPedidoRepository>();
        
        pedidoRepository.Setup(r =>
                r.BuscarPorIdAsync(pedido.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pedido?)null);
        
        var useCase = new PagarPedidoUseCase(pedidoRepository.Object);
        
        await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecutarAsync(pedido.Id));
    }
}