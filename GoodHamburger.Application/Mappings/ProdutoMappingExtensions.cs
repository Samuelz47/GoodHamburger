using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.Mappings;

public static class ProdutoMappingExtensions
{
    public static ProdutoDTO ToDTO(this Produto produto)
    {
        return new ProdutoDTO
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Preco = produto.Preco,
            Categoria = produto.Categoria.ToString()
        };
    }

    public static IEnumerable<ProdutoDTO> ToDTOList(this IEnumerable<Produto> produtos)
    {
        return produtos.Select(produto => produto.ToDTO());
    }
}