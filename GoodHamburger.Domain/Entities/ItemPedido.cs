namespace GoodHamburger.Domain.Entities;

public class ItemPedido
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal PrecoTotal => Quantidade * PrecoUnitario;
}