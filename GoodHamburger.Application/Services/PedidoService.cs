using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.Mappings;
using GoodHamburger.Application.Repositories;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _uow;

    public PedidoService(IUnitOfWork uow, IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository)
    {
        _uow = uow;
        _pedidoRepository = pedidoRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<PedidoDTO?> GetPedidoAsync(int pedidoId)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(pedidoId);
        if (pedido is null) return null;
        var pedidoDto = pedido.ToDto();
        return pedidoDto;
    }

    public async Task<IEnumerable<PedidoDTO>> GetPedidosAsync()
    {
        var pedidos = await _pedidoRepository.GetPedidosAsync();
        var pedidosDto = pedidos.ToDtoList();
        return pedidosDto;
    }

    public async Task<PedidoDTO?> CreatePedidoAsync(PedidoForRegistrationDTO pedidoForRegistration)
    {
        if (pedidoForRegistration.ProdutosId is null || !pedidoForRegistration.ProdutosId.Any())
            throw new ArgumentException("O pedido deve conter pelo menos um item.");
        
        var pedido = new Pedido();
            
        await CalcularPedidosAsync(pedido, pedidoForRegistration.ProdutosId);
        
        await _pedidoRepository.AddPedidoAsync(pedido);
        await _uow.SaveAsync();
        
        var pedidosDto = pedido.ToDto();
        return pedidosDto;
    }

    public async Task<PedidoDTO?> UpdatePedidoAsync(PedidoForUpdateDTO pedidoForUpdate)
    {
        if (pedidoForUpdate.ProdutosIds is null || !pedidoForUpdate.ProdutosIds.Any())
            throw new ArgumentException("A atualização do pedido deve conter pelo menos um item.");
        
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(pedidoForUpdate.Id);
        
        if(pedido is null) return null;
        
        await CalcularPedidosAsync(pedido, pedidoForUpdate.ProdutosIds);
        await _pedidoRepository.UpdatePedidoAsync(pedido);
        await _uow.SaveAsync();
        
        var pedidosDto = pedido.ToDto();
        return pedidosDto;
    }

    public async Task<bool> DeletePedidoAsync(int pedidoId)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(pedidoId);
        if(pedido is null) return false;
        
        await _pedidoRepository.DeletePedidoAsync(pedidoId);
        await _uow.SaveAsync();
        return true;
    }
    private async Task CalcularPedidosAsync(Pedido pedido, IEnumerable<int> produtosIds)
    {
        decimal subTotal = 0;
        var categorias = new HashSet<string>();
        var produtosDoPedido = new List<Produto>();
        bool adicionaCategorias = true;
        foreach (var item in produtosIds)
        {
            var produto = await _produtoRepository.GetProdutoById(item);
            if (produto == null) throw new ArgumentException($"O produto não existe no cardápio.");
            
            subTotal += produto.Preco;
            
            adicionaCategorias = categorias.Add(produto.Categoria.ToString());
            if (!adicionaCategorias) throw new ArgumentException("Pedido não pode conter itens de categorias iguais");
            
            produtosDoPedido.Add(produto);
        }
        
        pedido.SubTotal = subTotal;
        pedido.Total = subTotal;
        pedido.Items = produtosDoPedido;
        
        if (categorias.Count > 1) 
        {
            if (categorias.Contains("Sanduiche") && categorias.Contains("BatataFrita") && categorias.Contains("Refrigerante"))
            {
                pedido.Total = subTotal * 0.80m; 
            }
            else if (categorias.Contains("Sanduiche") && categorias.Contains("Refrigerante"))
            {
                pedido.Total = subTotal * 0.85m; 
            }
            else if (categorias.Contains("Sanduiche") && categorias.Contains("BatataFrita"))
            {
                pedido.Total = subTotal * 0.90m; 
            }
        }
    }
}