using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Repositories;

public interface IProdutoRepository
{
    Task<Produto?> GetProdutoById(int id);
    Task<IEnumerable<Produto>> GetAllProdutosAsync();
}