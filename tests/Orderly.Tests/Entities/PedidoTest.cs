using JetBrains.Annotations;
using Orderly.Domain.Entities;
using Orderly.Domain.Enums;
using Orderly.Domain.Exceptions;

namespace Orderly.Tests.Entities;

[TestSubject(typeof(Pedido))]
public class PedidoTest
{
    private Pedido CriarPedidoValido()
    {
        var produto = Produto.CriarProduto("Produto 1", 100m, 10);

        return Pedido.CriarPedido(new List<Produto> { produto });
    }
    
    [Fact]
    public void Criar_ComDadosValidos_DeveCriarPedido()
    {
        var p1 = Produto.CriarProduto("Produto 1", 100m, 10);
        var p2 = Produto.CriarProduto("Produto 2", 200m, 20);
        var p3 = Produto.CriarProduto("Produto 3", 300m, 30);

        List<Produto> produtos = new List<Produto> { p1, p2, p3 };

        var pedido = Pedido.CriarPedido(produtos);
    
        Assert.NotNull(pedido);
        Assert.NotEqual(Guid.Empty, pedido.Id);
        Assert.Equal(600m, pedido.ValorTotal);
        Assert.Equal(StatusPedido.Pendente, pedido.Status);
        Assert.Equal(3, pedido.Produtos.Count);
        Assert.NotEqual(default, pedido.DataPedido);
    }

    [Fact]
    public void Criar_SemProdutos_NaoDeveCriarPedido()
    {
        List<Produto> produtos = new List<Produto>();
        
        Assert.Throws<ExceptionPedidoDomain>(() => Pedido.CriarPedido(produtos));
    }

    [Fact]
    public void Criar_QuantidadeMaiorQueEstoque_NaoDeveCriarPedido()
    {
        var p1 = Produto.CriarProduto("Produto 1", 100m, 2);
        
        List<Produto> produtos = new List<Produto> { p1, p1, p1 };
        
        Assert.Throws<ExceptionPedidoDomain>(() => Pedido.CriarPedido(produtos));
    }
    
    [Fact]
    public void Pagar_StatusPendente_DeveMudarParaProcessando()
    {
        var pedido = CriarPedidoValido();

        pedido.Pagar();

        Assert.Equal(StatusPedido.Processando, pedido.Status);
    }

    [Fact]
    public void AprovarPagamento_StatusProcessando_DeveMudarParaConfirmado()
    {
        var pedido = CriarPedidoValido();

        pedido.Pagar();
        pedido.AprovarPagamento();

        Assert.Equal(StatusPedido.Confirmado, pedido.Status);
    }
    
    [Fact]
    public void RecusarPagamento_DeveDevolverEstoqueDosProdutos()
    {
        var produto = Produto.CriarProduto("Produto 1", 100m, 10);
        var produtos = new List<Produto> { produto, produto, produto };

        var pedido = Pedido.CriarPedido(produtos);

        pedido.Pagar();
        pedido.RecusarPagamento();

        Assert.Equal(10, produto.Estoque);
        Assert.Equal(StatusPedido.PagamentoRecusado, pedido.Status);
    }

    [Fact]
    public void IniciarPreparacao_StatusConfirmado_DeveMudarParaPreparacao()
    {
        var pedido = CriarPedidoValido();

        pedido.Pagar();
        pedido.AprovarPagamento();
        pedido.IniciarPreparacao();

        Assert.Equal(StatusPedido.Preparacao, pedido.Status);
    }

    [Fact]
    public void Enviar_StatusPreparacao_DeveMudarParaEnviado()
    {
        var pedido = CriarPedidoValido();

        pedido.Pagar();
        pedido.AprovarPagamento();
        pedido.IniciarPreparacao();

        pedido.Enviar();

        Assert.Equal(StatusPedido.Enviado, pedido.Status);
    }

    [Fact]
    public void Entregar_StatusEnviado_DeveMudarParaEntregue()
    {
        var pedido = CriarPedidoValido();

        pedido.Pagar();
        pedido.AprovarPagamento();
        pedido.IniciarPreparacao();
        pedido.Enviar();

        pedido.Entregar();

        Assert.Equal(StatusPedido.Entregue, pedido.Status);
    }
    
    [Fact]
    public void AprovarPagamento_StatusPendente_DeveLancarExcecao()
    {
        var pedido = CriarPedidoValido();

        Assert.Throws<ExceptionPedidoDomain>(() => pedido.AprovarPagamento());
    }
    
    [Fact]
    public void Entregar_StatusPendente_DeveLancarExcecao()
    {
        var pedido = CriarPedidoValido();

        Assert.Throws<ExceptionPedidoDomain>(() => pedido.Entregar());
    }

    [Fact]
    public void Enviar_StatusConfirmado_DeveLancarExcecao()
    {
        var pedido = CriarPedidoValido();

        pedido.Pagar();
        pedido.AprovarPagamento();

        Assert.Throws<ExceptionPedidoDomain>(() => pedido.Enviar());
    }
    
    [Fact]
    public void Cancelar_ProdutosRepetidos_DeveDevolverQuantidadeCorretaAoEstoque()
    {
        var produto = Produto.CriarProduto("Produto 1", 100m, 10);
        var produtos = new List<Produto> { produto, produto, produto };

        var pedido = Pedido.CriarPedido(produtos);

        pedido.Cancelar();

        Assert.Equal(10, produto.Estoque);
        Assert.Equal(StatusPedido.Cancelado, pedido.Status);
    }
    
    [Fact]
    public void Cancelar_StatusEntregue_DeveLancarExcecao()
    {
        var pedido = CriarPedidoValido();

        pedido.Pagar();
        pedido.AprovarPagamento();
        pedido.IniciarPreparacao();
        pedido.Enviar();
        pedido.Entregar();

        Assert.Throws<ExceptionPedidoDomain>(() => pedido.Cancelar());
    }
}