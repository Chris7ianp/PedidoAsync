using System.Runtime.CompilerServices;

namespace PedidoAsync.Application
{
    public class PedidoDto
    {
        public Guid Id { get; set; }
        public int NumeroPedido { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataPedido { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataProcessamento { get; set; }

    }
}
