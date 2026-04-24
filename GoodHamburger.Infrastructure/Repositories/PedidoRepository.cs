using GoodHamburger.Application.Repositories;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Pedido?> GetPedidoByIdAsync(int id)
    {
        return await _context.Pedidos.Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Pedido>> GetPedidosAsync()
    {
        return await _context.Pedidos.Include(p => p.Items)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Pedido> AddPedidoAsync(Pedido pedido)
    {
        await _context.Pedidos.AddAsync(pedido);
        return pedido;
    }

    public async Task UpdatePedidoAsync(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
    }

    public async Task DeletePedidoAsync(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido != null) _context.Pedidos.Remove(pedido);
    }
}