using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoAsync.Application.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublicarAsync<T>(T mensagem);
    }
}
