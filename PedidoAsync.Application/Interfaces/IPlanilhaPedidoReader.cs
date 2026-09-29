using PedidoAsync.Application.DTOs;
using PedidoAsync.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoAsync.Application.Interfaces
{
    public interface IPlanilhaPedidoReader
    {
        Task<(List<CriarPedidoImportacaoDto> Pedidos, List<string> Erros)>
            LerAsync(Stream arquivo);
    }
}
