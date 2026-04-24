using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IPedidoService
{
    Task<PedidoDTO?> GetPedidoAsync(int pedidoId);
    Task<IEnumerable<PedidoDTO>> GetPedidosAsync();
    Task<PedidoDTO?> CreatePedidoAsync(PedidoForRegistrationDTO pedidoForRegistration);
    Task<PedidoDTO?> UpdatePedidoAsync(PedidoForUpdateDTO pedidoForUpdate);
    Task<bool> DeletePedidoAsync(int pedidoId);
}