using Moq;
using Xunit;
using FluentAssertions;
using GoodHamburger.Application.Services;
using GoodHamburger.Application.Repositories;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace GoodHamburger.Tests;

public class PedidoServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IPedidoRepository> _pedidoRepoMock;
    private readonly Mock<IProdutoRepository> _produtoRepoMock;
    private readonly PedidoService _pedidoService;

    public PedidoServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _pedidoRepoMock = new Mock<IPedidoRepository>();
        _produtoRepoMock = new Mock<IProdutoRepository>();

        _pedidoService = new PedidoService(_uowMock.Object, _pedidoRepoMock.Object, _produtoRepoMock.Object);
    }

    [Fact]
    public async Task CreatePedidoAsync_WithDuplicatedCategory_ShouldThrowException()
    {
        var produto1 = new Produto { Id = 1, Nome = "X Burger", Categoria = CategoriaProduto.Sanduiche, Preco = 5.0m };
        var produto2 = new Produto { Id = 2, Nome = "X Egg", Categoria = CategoriaProduto.Sanduiche, Preco = 4.5m };

        _produtoRepoMock.Setup(repo => repo.GetProdutoById(1)).ReturnsAsync(produto1);
        _produtoRepoMock.Setup(repo => repo.GetProdutoById(2)).ReturnsAsync(produto2);

        var dto = new PedidoForRegistrationDTO { ProdutosId = new List<int> { 1, 2 } };

        Func<Task> action = async () => await _pedidoService.CreatePedidoAsync(dto);

        await action.Should().ThrowAsync<ArgumentException>().WithMessage("Pedido não pode conter itens de categorias iguais");
    }

    [Fact]
    public async Task CreatePedidoAsync_WithSanduicheBatataRefri_ShouldApply20PercentDiscount()
    {
        var p1 = new Produto { Id = 1, Nome = "X Burger", Categoria = CategoriaProduto.Sanduiche, Preco = 5.0m };
        var p2 = new Produto { Id = 2, Nome = "Batata", Categoria = CategoriaProduto.BatataFrita, Preco = 2.0m };
        var p3 = new Produto { Id = 3, Nome = "Refri", Categoria = CategoriaProduto.Refrigerante, Preco = 2.5m };

        _produtoRepoMock.Setup(repo => repo.GetProdutoById(1)).ReturnsAsync(p1);
        _produtoRepoMock.Setup(repo => repo.GetProdutoById(2)).ReturnsAsync(p2);
        _produtoRepoMock.Setup(repo => repo.GetProdutoById(3)).ReturnsAsync(p3);

        var dto = new PedidoForRegistrationDTO { ProdutosId = new List<int> { 1, 2, 3 } };

        var result = await _pedidoService.CreatePedidoAsync(dto);

        result.Should().NotBeNull();
        result.SubTotal.Should().Be(9.5m);
        result.Total.Should().Be(7.6m);
    }

    [Fact]
    public async Task CreatePedidoAsync_WithSanduicheRefri_ShouldApply15PercentDiscount()
    {
        var p1 = new Produto { Id = 1, Nome = "X Burger", Categoria = CategoriaProduto.Sanduiche, Preco = 5.0m };
        var p3 = new Produto { Id = 3, Nome = "Refri", Categoria = CategoriaProduto.Refrigerante, Preco = 2.5m };

        _produtoRepoMock.Setup(repo => repo.GetProdutoById(1)).ReturnsAsync(p1);
        _produtoRepoMock.Setup(repo => repo.GetProdutoById(3)).ReturnsAsync(p3);

        var dto = new PedidoForRegistrationDTO { ProdutosId = new List<int> { 1, 3 } };

        var result = await _pedidoService.CreatePedidoAsync(dto);

        result.Should().NotBeNull();
        result.SubTotal.Should().Be(7.5m);
        result.Total.Should().Be(6.375m);
    }

    [Fact]
    public async Task CreatePedidoAsync_WithSanduicheBatata_ShouldApply10PercentDiscount()
    {
        var p1 = new Produto { Id = 1, Nome = "X Burger", Categoria = CategoriaProduto.Sanduiche, Preco = 5.0m };
        var p2 = new Produto { Id = 2, Nome = "Batata", Categoria = CategoriaProduto.BatataFrita, Preco = 2.0m };

        _produtoRepoMock.Setup(repo => repo.GetProdutoById(1)).ReturnsAsync(p1);
        _produtoRepoMock.Setup(repo => repo.GetProdutoById(2)).ReturnsAsync(p2);

        var dto = new PedidoForRegistrationDTO { ProdutosId = new List<int> { 1, 2 } };

        var result = await _pedidoService.CreatePedidoAsync(dto);

        result.Should().NotBeNull();
        result.SubTotal.Should().Be(7.0m);
        result.Total.Should().Be(6.3m);
    }

    [Fact]
    public async Task UpdatePedidoAsync_WithValidItems_ShouldUpdateAndRecalculate()
    {
        var pedidoExistente = new Pedido { Id = 1, SubTotal = 5.0m, Total = 5.0m, Items = new List<Produto>() };
        var p1 = new Produto { Id = 1, Nome = "X Burger", Categoria = CategoriaProduto.Sanduiche, Preco = 5.0m };
        var p2 = new Produto { Id = 2, Nome = "Batata", Categoria = CategoriaProduto.BatataFrita, Preco = 2.0m };

        _pedidoRepoMock.Setup(repo => repo.GetPedidoByIdAsync(1)).ReturnsAsync(pedidoExistente);
        _produtoRepoMock.Setup(repo => repo.GetProdutoById(1)).ReturnsAsync(p1);
        _produtoRepoMock.Setup(repo => repo.GetProdutoById(2)).ReturnsAsync(p2);

        var dto = new PedidoForUpdateDTO { ProdutosIds = new List<int> { 1, 2 } };

        var result = await _pedidoService.UpdatePedidoAsync(1, dto);

        result.Should().NotBeNull();
        result.SubTotal.Should().Be(7.0m);
        result.Total.Should().Be(6.3m); 
        _pedidoRepoMock.Verify(repo => repo.UpdatePedidoAsync(It.IsAny<Pedido>()), Times.Once);
        _uowMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatePedidoAsync_WithNonExistentPedidoId_ShouldThrowKeyNotFoundException()
    {
        _pedidoRepoMock.Setup(repo => repo.GetPedidoByIdAsync(99)).ReturnsAsync((Pedido?)null);

        var dto = new PedidoForUpdateDTO { ProdutosIds = new List<int> { 1 } };

        Func<Task> action = async () => await _pedidoService.UpdatePedidoAsync(99, dto);

        await action.Should().ThrowAsync<KeyNotFoundException>().WithMessage("O pedido não foi localizado.");
    }

    [Fact]
    public async Task UpdatePedidoAsync_WithNoItems_ShouldThrowException()
    {
        var dto = new PedidoForUpdateDTO { ProdutosIds = new List<int>() };

        Func<Task> action = async () => await _pedidoService.UpdatePedidoAsync(1, dto);

        await action.Should().ThrowAsync<ArgumentException>().WithMessage("A atualização do pedido deve conter pelo menos um item.");
    }
}
