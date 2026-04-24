using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Repositories;

public interface IPedidoRepository
{
    Task<Pedido?> GetPedidoByIdAsync(int id);
    Task<IEnumerable<Pedido>> GetPedidosAsync();
    Task<Pedido> AddPedidoAsync(Pedido pedido);
    Task UpdatePedidoAsync(Pedido pedido);
    Task DeletePedidoAsync(int id);
}