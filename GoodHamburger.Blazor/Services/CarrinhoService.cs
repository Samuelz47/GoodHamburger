using GoodHamburger.Blazor.Models;
using System.Net.Http.Json;

namespace GoodHamburger.Blazor.Services;

public class CarrinhoService
{
    private readonly HttpClient _httpClient;
    private readonly List<Produto> _itens = new();

    public event Action? OnChange;

    public CarrinhoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public IReadOnlyList<Produto> Itens => _itens.AsReadOnly();

    public void AdicionarProduto(Produto produto)
    {
        // Regra de negócio: não pode categorias duplicadas no mesmo pedido
        if (_itens.Any(p => p.Categoria == produto.Categoria))
        {
            throw new InvalidOperationException("Você já adicionou um item desta categoria ao pedido.");
        }

        _itens.Add(produto);
        NotifyStateChanged();
    }

    public void RemoverProduto(int produtoId)
    {
        var item = _itens.FirstOrDefault(p => p.Id == produtoId);
        if (item != null)
        {
            _itens.Remove(item);
            NotifyStateChanged();
        }
    }

    public void LimparCarrinho()
    {
        _itens.Clear();
        NotifyStateChanged();
    }

    public async Task<bool> FinalizarPedidoAsync()
    {
        if (!_itens.Any()) return false;

        var dto = new { ProdutosId = _itens.Select(p => p.Id).ToList() };
        var response = await _httpClient.PostAsJsonAsync("api/pedido", dto);

        if (response.IsSuccessStatusCode)
        {
            LimparCarrinho();
            return true;
        }

        return false;
    }

    public async Task<bool> AtualizarPedidoAsync(int pedidoId)
    {
        if (!_itens.Any()) return false;

        var dto = new { ProdutosIds = _itens.Select(p => p.Id).ToList() };
        var response = await _httpClient.PutAsJsonAsync($"api/pedido/{pedidoId}", dto);

        if (response.IsSuccessStatusCode)
        {
            LimparCarrinho();
            return true;
        }

        return false;
    }

    public async Task<bool> ExcluirPedidoAsync(int pedidoId)
    {
        var response = await _httpClient.DeleteAsync($"api/pedido/{pedidoId}");
        return response.IsSuccessStatusCode;
    }

    public void CarregarItensEduicao(IEnumerable<Produto> itens)
    {
        _itens.Clear();
        foreach (var item in itens)
        {
            _itens.Add(item);
        }
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
