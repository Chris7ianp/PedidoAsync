using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoAsync.Domain.Events
{
    public class PedidoCriadoEvent
    {
        public Guid PedidoId { get; set; }
        public Guid ImportacaoId { get; set; }
        public int NumeroPedido { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataPedido { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
