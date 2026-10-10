using Orderly.Domain.Enums;
using Orderly.Domain.Exceptions;

namespace Orderly.Domain.Entities
{
    public class Pedido
    {
        public Guid Id  { get; private set; }
        public List<Produto> Produtos { get; private set; }
        public decimal ValorTotal { get; private set; }
        public StatusPedido Status { get; private set; }
        public DateTime DataPedido { get; private set; }
        
        private Pedido() { }

        public static Pedido CriarPedido(List<Produto> produtos)
        {
            if (produtos is null || produtos.Count == 0)
                throw new ExceptionPedidoDomain("Nenhum produto selecionado");

            var itensAgrupados = produtos
                .GroupBy(p => p.Id)
                .Select(grupo => new
                {
                    Produto = grupo.First(),
                    Quantidade = grupo.Count()
                })
                .ToList();

            foreach (var item in itensAgrupados)
            {
                if (item.Produto.Estoque < item.Quantidade)
                    throw new ExceptionPedidoDomain($"Estoque insuficiente para o produto {item.Produto.Nome}");
            }

            foreach (var item in itensAgrupados)
            {
                item.Produto.AtualizarEstoque(item.Quantidade);
            }

            return new Pedido()
            {
                Id = Guid.NewGuid(),
                Produtos = produtos,
                ValorTotal = CalcularValorTotal(produtos),
                Status = StatusPedido.Pendente,
                DataPedido = DateTime.Now
            };
        }

        public void Pagar()
        {
            if (Status != StatusPedido.Pendente)
                throw new ExceptionPedidoDomain("Ciclo de vida do Status indisponivel");

            Status = StatusPedido.Processando;
        }

        public void AprovarPagamento()
        {
            if (Status != StatusPedido.Processando)
                throw new ExceptionPedidoDomain("Ciclo de vida do Status indisponivel");

            Status = StatusPedido.Confirmado;
        }

        public void RecusarPagamento()
        {
            if (Status != StatusPedido.Processando)
                throw new ExceptionPedidoDomain("Ciclo de vida do Status indisponivel");

            Status = StatusPedido.PagamentoRecusado;
        }

        public void IniciarPreparacao()
        {
            if (Status != StatusPedido.Confirmado)
                throw new ExceptionPedidoDomain("Ciclo de vida do Status indisponivel");

            Status = StatusPedido.Preparacao;
        }

        public void Enviar()
        {
            if (Status != StatusPedido.Preparacao)
                throw new ExceptionPedidoDomain("Ciclo de vida do Status indisponivel");

            Status = StatusPedido.Enviado;
        }

        public void Entregar()
        {
            if (Status != StatusPedido.Enviado)
                throw new ExceptionPedidoDomain("Ciclo de vida do Status indisponivel");

            Status = StatusPedido.Entregue;
        }

        public void Cancelar()
        {
            if (Status is not (StatusPedido.Pendente or 
                            StatusPedido.Processando or
                            StatusPedido.Confirmado or
                            StatusPedido.Preparacao))
                throw new ExceptionPedidoDomain("Ciclo de vida do Status indisponivel");
            
            Status = StatusPedido.Cancelado;
        }

        private static decimal CalcularValorTotal(List<Produto> produtos)
        {
            return produtos
                .Sum(p => p.Preco);
        }
    }
}