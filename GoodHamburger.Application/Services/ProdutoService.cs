using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.Mappings;
using GoodHamburger.Application.Repositories;

namespace GoodHamburger.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<IEnumerable<ProdutoDTO>> GetProdutosAsync()
    {
        var produtos = await _produtoRepository.GetAllProdutosAsync();
        var produtosDto = produtos.ToDTOList();
        return produtosDto;
    }

    public async Task<ProdutoDTO?> GetProdutoByIdAsync(int id)
    {
        var produto = await _produtoRepository.GetProdutoById(id);
        if (produto is null) return null;
        var produtoDto = produto.ToDTO();
        return produtoDto;
    }
}