using GoodHamburger.Application.Repositories;
using GoodHamburger.Infrastructure.Data;

namespace GoodHamburger.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveAsync()
    {
        // Salva as alterações e retorna true se pelo menos uma linha foi afetada no banco
        return await _context.SaveChangesAsync() > 0;
    }
}