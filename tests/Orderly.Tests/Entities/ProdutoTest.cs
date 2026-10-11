using JetBrains.Annotations;
using Orderly.Domain.Entities;
using Orderly.Domain.Exceptions;

namespace Orderly.Tests.Entities;

[TestSubject(typeof(Produto))]
public class ProdutoTest
{

    private Produto CriarProdutoValido()
    {
        var produto = Produto.CriarProduto("Produto 1", 100m, 10);

        return produto;
    }

    [Fact]
    public void Criar_ComDadosValidos_DeveCriarProduto()
    {
        var produto = CriarProdutoValido();

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
    public void AtualizarEstoque_ComDadosValidos_DeveAtualizarEstoque()
    {
        var produto = CriarProdutoValido();
        
        produto.AtualizarEstoque(8);
        
        Assert.Equal(2, produto.Estoque);
    }

    [Fact]
    public void AtualizarEstoque_ComQuantidadeMaiorQueEstoque_NaoDeveAtualizarEstoque()
    {
        var produto = CriarProdutoValido();
        
        Assert.Throws<ExceptionProdutoDomain>(() => produto.AtualizarEstoque(11));
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    public void AtualizarEstoque_ComDadosInvalidos_NaoDeveAtualizarEstoque(int quantidade)
    {
        var produto = CriarProdutoValido();
        
        Assert.Throws<ExceptionProdutoDomain>(() => produto.AtualizarEstoque(quantidade));
    }

    [Fact]
    public void ReporEstoque_ComDadosValidos_DeveReporEstoque()
    {
        var produto = CriarProdutoValido();
        
        produto.ReporEstoque(10);
        
        Assert.Equal(20, produto.Estoque);
    }
    
    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    public void ReporEstoque_ComDadosInvalidos_NaoDeveAtualizarEstoque(int quantidade)
    {
        var produto = CriarProdutoValido();
        
        Assert.Throws<ExceptionProdutoDomain>(() => produto.ReporEstoque(quantidade));
    }
}
