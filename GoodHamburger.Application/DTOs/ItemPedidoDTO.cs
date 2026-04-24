namespace GoodHamburger.Application.DTOs;

public class ItemPedidoDTO
{
    public int Id { get; set; }
    public string ProdutoNome { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal PrecoTotal { get; set; }
}