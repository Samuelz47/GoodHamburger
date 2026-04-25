using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutoController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var produto = await _produtoService.GetProdutoByIdAsync(id);
        
        if (produto is null) return NotFound("Produto não encontrado");
        return Ok(produto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var produtos = await _produtoService.GetProdutosAsync();
        return Ok(produtos);
    }
}