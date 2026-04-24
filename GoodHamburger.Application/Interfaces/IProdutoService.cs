using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoDTO>> GetProdutosAsync();
    Task<ProdutoDTO?> GetProdutoByIdAsync(int id);
}