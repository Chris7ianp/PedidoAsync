using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoAsync.Application.DTOs
{
    public class ImportacaoDto
    {
        public Guid Id { get; set; }

        public string NomeArquivo { get; set; } = string.Empty;

        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public int TotalRegistros { get; set; }

        public int Processados { get; set; }

        public int Erros { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
