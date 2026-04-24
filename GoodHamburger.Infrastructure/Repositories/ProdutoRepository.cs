using GoodHamburger.Application.Repositories;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Produto?> GetProdutoById(int id)
    {
        return await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<IEnumerable<Produto>> GetAllProdutosAsync()
    {
        return await _context.Produtos.AsNoTracking().ToListAsync();
    }
}