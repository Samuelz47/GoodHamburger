namespace GoodHamburger.Domain.Entities;

public class Pedido
{
    public int Id { get; set; }
    public List<Produto> Items { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
}