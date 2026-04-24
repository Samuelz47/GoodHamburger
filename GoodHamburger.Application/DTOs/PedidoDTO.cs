using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.DTOs;

public class PedidoDTO
{
    public int Id { get; set; }
    public List<ItemPedido> Items { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
}