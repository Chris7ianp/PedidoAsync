using PedidoAsync.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoAsync.Domain.Entities
{
    public class Importacao
    {
        public Guid Id { get; private set; }
        public string NomeArquivo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int TotalRegistros { get; set; }
        public int Processados { get; set; }
        public int  Erros { get; set; }
        public StatusImportacao Status { get; set; }

        private Importacao()
        {

        }

        public Importacao(string nomeArquivo, int totalRegistros)
        {
            Id = Guid.NewGuid();
            NomeArquivo = nomeArquivo;
            DataInicio = DateTime.Now;
            TotalRegistros = totalRegistros;
            Processados = 0;
            Status = StatusImportacao.Pendente;
        }

        public void IniciarProcessamento()
        {
            Status = StatusImportacao.Processando;
        }

        public void IniciarImportacao()
        {
            Status = StatusImportacao.Processando;
        }

        public void RegistrarProcessamento()
        {
            Processados++;
            AtualizarStatus();
        }
        public void RegistrarErro()
        {
            Erros++;
            AtualizarStatus();
        }
        private void AtualizarStatus()
        {
            var totalProcessado = Processados + Erros;

            if (totalProcessado < TotalRegistros)
            {
                Status = StatusImportacao.Processando;
                return;
            }

            DataFim = DateTime.UtcNow;

            Status = Erros > 0 ? StatusImportacao.ConcluidaComErros : StatusImportacao.Concluida;
        }
    }
}
