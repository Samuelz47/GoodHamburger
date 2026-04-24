namespace GoodHamburger.Application.DTOs;

public class PedidoDTO
{
    public int Id { get; set; }
    public List<ProdutoDTO> Items { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
}