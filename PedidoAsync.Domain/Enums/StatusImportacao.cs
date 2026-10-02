using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoAsync.Domain.Enums
{
    public enum StatusImportacao
    {
        Pendente = 1,
        Processando = 2,
        Concluida = 3,
        ConcluidaComErros = 4
    }
}
