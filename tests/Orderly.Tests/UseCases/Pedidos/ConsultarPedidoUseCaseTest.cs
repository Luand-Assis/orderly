using JetBrains.Annotations;
using Moq;
using Orderly.Application.Exceptions;
using Orderly.Application.Interfaces;
using Orderly.Application.UseCases.Pedidos;
using Orderly.Domain.Entities;

namespace Orderly.Tests.UseCases.Pedidos;

[TestSubject(typeof(ConsultarPedidoUseCase))]
public class ConsultarPedidoUseCaseTest
{

    [Fact]
    public async Task Executar_ComPedidoExistente_DeveRetornarDadosDoPedido()
    {
        var produto1 = Produto.CriarProduto("Produto 1", 100m, 10);
        var produto2 = Produto.CriarProduto("Produto 2", 200m, 10);

        var pedido = Pedido.CriarPedido(new List<Produto> { produto1, produto2 });

        var pedidoRepository = new Mock<IPedidoRepository>();

        pedidoRepository
            .Setup(r => r.BuscarPorIdAsync(
                pedido.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedido);

        var useCase = new ConsultarPedidoUseCase(pedidoRepository.Object);
        
        var resultado = await useCase.ExecutarAsync(pedido.Id);
        
        Assert.NotNull(resultado);
        Assert.Equal(pedido.Id, resultado.Id);
        Assert.Equal(pedido.Status.ToString(), resultado.Status);
        Assert.Equal(300m, resultado.ValorTotal);
        Assert.Equal(pedido.DataPedido, resultado.DataHora);
        Assert.Equal(2, resultado.Itens.Count);

        pedidoRepository.Verify(
            r => 
                r.BuscarPorIdAsync(pedido.Id, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task Executar_ComPedidoInexistente_DeveLancarExcecao()
    {
        var pedidoId = Guid.NewGuid();

        var pedidoRepository = new Mock<IPedidoRepository>();

        pedidoRepository.Setup(r => 
                r.BuscarPorIdAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pedido?)null);

        var useCase = new ConsultarPedidoUseCase(pedidoRepository.Object);
        
        var excecao = await Assert.ThrowsAsync<ExceptionPedidoApplication>(() => useCase.ExecutarAsync(pedidoId));

        Assert.Equal("Pedido indisponivel", excecao.Message);
    }

    [Fact]
    public async Task Executar_ComProdutosRepetidos_DeveAgruparEContarItens()
    {
        var produto = Produto.CriarProduto("Produto 1", 100m, 10);

        var pedido = Pedido.CriarPedido(new List<Produto> { produto, produto, produto });

        var pedidoRepository = new Mock<IPedidoRepository>();

        pedidoRepository.Setup(r => 
                r.BuscarPorIdAsync(pedido.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedido);

        var useCase = new ConsultarPedidoUseCase(pedidoRepository.Object);
        
        var resultado = await useCase.ExecutarAsync(pedido.Id);
        
        var item = Assert.Single(resultado.Itens);

        Assert.Equal("Produto 1", item.Nome);
        Assert.Equal(3, item.Quantidade);
        Assert.Equal(100m, item.PrecoUnitario);
        Assert.Equal(300m, resultado.ValorTotal);
    }

    [Fact]
    public async Task Executar_DeveBuscarPedidoComIdInformado()
    {
        var produto = Produto.CriarProduto("Produto 1", 100m, 10);
        var pedido = Pedido.CriarPedido(new List<Produto> { produto });

        var pedidoRepository = new Mock<IPedidoRepository>();

        pedidoRepository.Setup(r => 
                r.BuscarPorIdAsync(pedido.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedido);

        var useCase = new ConsultarPedidoUseCase(pedidoRepository.Object);
        
        await useCase.ExecutarAsync(pedido.Id);
        
        pedidoRepository.Verify(r => 
                r.BuscarPorIdAsync(pedido.Id, It.IsAny<CancellationToken>()));
    }
}