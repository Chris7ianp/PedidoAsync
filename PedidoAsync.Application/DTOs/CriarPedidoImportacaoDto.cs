using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoAsync.Application.DTOs
{
    public class CriarPedidoImportacaoDto
    {
        public int NumeroPedido { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataPedido { get; set; }
    }
}
