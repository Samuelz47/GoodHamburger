using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Mappings;

public static class PedidoMappingExtensions
{
    public static PedidoDTO ToDto(this Pedido pedido)
    {
        return new PedidoDTO
        {
            Id = pedido.Id,
            Items = pedido.Items.Select(item => new ProdutoDTO() 
            {
                Id = item.Id,
                Nome = item.Nome,
                Categoria = item.Categoria.ToString(),
                Preco = item.Preco,
            }).ToList(),
            SubTotal = pedido.SubTotal,
            Total = pedido.Total
        };
    }
    
    public static IEnumerable<PedidoDTO> ToDtoList(this IEnumerable<Pedido> pedidos)
    {
        return pedidos.Select(pedido => pedido.ToDto());
    }
}