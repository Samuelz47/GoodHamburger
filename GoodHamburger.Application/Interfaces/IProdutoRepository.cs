using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Interfaces;

public interface IProdutoRepository
{
    Task<Produto?> GetProdutoByName(string nome);
}