using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoAsync.Application.DTOs
{
    public class ResultadoImportacaoDto
    {
        public int TotalRegistros { get; set; }
        public int RegistrosImportados { get; set; }
        public int RegistrosComErro { get; set; }
        public List<string> Erros { get; set; }
    }
}
