namespace GoodHamburger.Blazor.Models;

public class Pedido
{
    public int Id { get; set; }
    public List<Produto> Items { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }

    public double DescontoPercentual
    {
        get
        {
            if (SubTotal == 0) return 0;
            var desconto = 1 - (Total / SubTotal);
            return (double)Math.Round(desconto * 100, 2);
        }
    }
}

