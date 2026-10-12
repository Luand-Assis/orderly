using JetBrains.Annotations;
using Moq;
using Orderly.Application.Exceptions;
using Orderly.Application.Interfaces;
using Orderly.Application.UseCases.Pedidos;
using Orderly.Domain.Entities;
using Orderly.Domain.Enums;

namespace Orderly.Tests.UseCases.Pedidos;

[TestSubject(typeof(CancelarPedidoUseCase))]
public class CancelarPedidoUseCaseTest
{
    [Fact]
    public async void Cancelar_ComDadosValidos_DeveCancelarOPedido()
    {
        var produto1 = Produto.CriarProduto("produto 1", 100m, 5);
        var produto2 = Produto.CriarProduto("produto 2", 200m, 3);

        List<Produto> produtos = new List<Produto>() { produto1, produto2 };
        
        var pedido = Pedido.CriarPedido(produtos);
        
        var pedidoRepository = new Mock<IPedidoRepository>();

        pedidoRepository.Setup(r =>
                r.BuscarPorIdAsync(pedido.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedido);

        var useCase = new CancelarPedidoUseCase(pedidoRepository.Object);

        await useCase.ExecutarAsync(pedido.Id);
        
        pedidoRepository.Verify(r =>
            r.AtualizarAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()));
        
        Assert.Equal(StatusPedido.Cancelado, pedido.Status);
    }

    [Fact]
    public async void Cancelar_ComPedidoInexistente_NaoDeveCancelar()
    {
        var produto1 = Produto.CriarProduto("produto 1", 100m, 5);
        var produto2 = Produto.CriarProduto("produto 2", 200m, 3);

        List<Produto> produtos = new List<Produto>() { produto1, produto2 };
        
        var pedido = Pedido.CriarPedido(produtos);
            
        var pedidoRepository = new Mock<IPedidoRepository>();
        
        pedidoRepository.Setup(r =>
                r.BuscarPorIdAsync(pedido.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pedido?)null);
        
        var useCase = new CancelarPedidoUseCase(pedidoRepository.Object);
        
        await Assert.ThrowsAsync<ExceptionPedidoApplication>(() => useCase.ExecutarAsync(pedido.Id));
    }
}