using System.Net.Http.Json;
using GoodHamburger.Blazor.Models;

namespace GoodHamburger.Blazor.Services;

public class ProdutoService
{
    private readonly HttpClient _httpClient;

    public ProdutoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Produto>> GetProdutosAsync()
    {
        try
        {
            var produtos = await _httpClient.GetFromJsonAsync<List<Produto>>("api/produto");
            return produtos ?? new List<Produto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar produtos: {ex.Message}");
            return new List<Produto>();
        }
    }
}

