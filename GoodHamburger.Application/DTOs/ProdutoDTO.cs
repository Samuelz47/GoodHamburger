using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.DTOs;

public class ProdutoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public CategoriaProduto Categoria { get; set; }
    public decimal Preco { get; set; }
}