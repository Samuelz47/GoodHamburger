namespace GoodHamburger.Application.DTOs;

public class PedidoForUpdateDTO
{
    public int Id { get; set; }
    public List<int> ProdutosIds { get; set; }
}