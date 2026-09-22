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
        public string Cliente { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
