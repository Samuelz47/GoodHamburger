using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }
    
    [HttpGet("{id}", Name = "GetPedido")]
    public async Task<IActionResult> Get(int id)
    {
        var pedido = await _pedidoService.GetPedidoAsync(id);
        if (pedido is null) return NotFound("Pedido não encontrado");
        return Ok(pedido);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pedidos = await _pedidoService.GetPedidosAsync();
        return Ok(pedidos);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePedido([FromBody] PedidoForRegistrationDTO pedidoRegister)
    {
        var pedido = await _pedidoService.CreatePedidoAsync(pedidoRegister);
        // Exceções tratadas através de middleware, então se chegar aqui é porque o pedido foi criado com sucesso
        return CreatedAtRoute("GetPedido", new { id = pedido.Id }, pedido);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePedido(int id, [FromBody] PedidoForUpdateDTO pedido)
    {
        var pedidoAtualizado = await _pedidoService.UpdatePedidoAsync(id, pedido);
        return Ok(pedidoAtualizado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePedido(int id)
    {
        var pedidoDeletado = await _pedidoService.DeletePedidoAsync(id);
        if (!pedidoDeletado) return BadRequest("Pedido não encontrado");
        return NoContent();
    }
}