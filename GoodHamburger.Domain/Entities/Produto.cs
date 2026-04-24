using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public CategoriaProduto Categoria { get; set; }
    public decimal Preco { get; set; }
}