using PedidoAsync.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoAsync.Application.Interfaces
{
    public interface IImportacaoRepository
    {
        Task AdicionarAsync(Importacao importacao);
        Task<Importacao?> ObterPorIdAsync(Guid id);
        Task AtualizarAsync(Importacao importacao);
        Task<List<Importacao>> ObterTodosAsync();

    }
}
