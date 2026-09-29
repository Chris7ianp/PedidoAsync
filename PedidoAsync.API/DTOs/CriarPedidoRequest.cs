namespace PedidoAsync.API.DTOs;

public class CriarPedidoRequest
{
    public int NumeroPedido { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public DateTime DataPedido { get; set; }
}