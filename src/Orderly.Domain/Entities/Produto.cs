using Orderly.Domain.Exceptions;

namespace Orderly.Domain.Entities
{
    public class Produto
    {
        public Guid Id { get; private set; }
        public string Nome { get; set; }
        public decimal Preco  { get; private set; }
        public int Estoque { get; private set; }
        
        private Produto() {}
        
        public static Produto CriarProduto(string nome, decimal preco, int estoque)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ExceptionProdutoDomain("Produto deve ter nome");

            if (preco <= 0.0m)
                throw new ExceptionProdutoDomain("Produto deve ter preco valido");

            if (estoque <= 0)
                throw new ExceptionProdutoDomain("Estoque deve ser valido");
        
            return new Produto()
            {
                Id = Guid.NewGuid(),
                Nome = nome,
                Preco = preco,
                Estoque = estoque
            };
        }

        public void AtualizarEstoque(int quantidade)
        {
            if (Estoque < quantidade)
                throw new ExceptionProdutoDomain("Quantidade maior que estoque");
            
            Estoque -= quantidade;
        }
    }
}