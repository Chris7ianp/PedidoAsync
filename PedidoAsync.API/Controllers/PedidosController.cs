using Microsoft.AspNetCore.Mvc;
using PedidoAsync.Application;
using PedidoAsync.Application.Services;

namespace PedidoAsync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly PedidoService _pedidoService;

    public PedidosController(PedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromBody] CriarPedidoRequest request)
    {
        var pedido = await _pedidoService.CriarAsync(
            request.Cliente,
            request.Valor);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = pedido.Id },
            pedido);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var pedidos = await _pedidoService.ObterTodosAsync();

        return Ok(pedidos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var pedido = await _pedidoService.ObterPorIdAsync(id);

        if (pedido is null)
        {
            return NotFound();
        }

        return Ok(pedido);
    }
}

public class CriarPedidoRequest
{
    public string Cliente { get; set; } = string.Empty;

    public decimal Valor { get; set; }
}