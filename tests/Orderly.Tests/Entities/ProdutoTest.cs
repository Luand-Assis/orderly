using JetBrains.Annotations;
using Orderly.Domain.Entities;
using Orderly.Domain.Exceptions;

namespace Orderly.Tests.Entities;

[TestSubject(typeof(Produto))]
public class ProdutoTest
{

    [Fact]
    public void Criar_ComDadosValidos_DeveCriarProduto()
    {
        var produto = Produto.CriarProduto("Produto 1", 100m, 10);

        Assert.Equal("Produto 1", produto.Nome);
        Assert.Equal(100m, produto.Preco);
        Assert.Equal(10, produto.Estoque);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Criar_ComNomeVazio_NaoDeveCriarProduto(string? nome)
    {
        Assert.Throws<ExceptionProdutoDomain>(() => Produto.CriarProduto(nome, 100m, 10));
    }

    [Fact]
    public void Criar_ComValorNegativo_NaoDeveCriarProduto()
    {
        Assert.Throws<ExceptionProdutoDomain>(() => Produto.CriarProduto("Produto 1", -100m, 10));
    }

    [Fact]
    public void Atualizar_ComDadosValidos_DeveAtualizarEstoque()
    {
        var produto = Produto.CriarProduto("Produto 1", 100m, 10);
        
        produto.AtualizarEstoque(8);
        
        Assert.Equal(2, produto.Estoque);
    }

    [Fact]
    public void Atualizar_ComQuantidadeMaiorQueEstoque_NaoDeveAtualizarEstoque()
    {
        var produto = Produto.CriarProduto("Produto 1", 100m, 10);
        
        Assert.Throws<ExceptionProdutoDomain>(() => produto.AtualizarEstoque(11));
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    public void Atualizar_ComDadosInvalidos_NaoDeveAtualizarEstoque(int quantidade)
    {
        var produto = Produto.CriarProduto("Produto 1", 100m, 10);
        
        Assert.Throws<ExceptionProdutoDomain>(() => produto.AtualizarEstoque(quantidade));
    }
}
