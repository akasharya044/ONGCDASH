using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.Processes
{
    public interface IBus
    {
        Task SendAsync<T>(string queue, T message);
        Task CreateExchange<T>(string exchangename, T message);
        Task ReceiveAsync<T>(string queue, Action<string> onMessage);
    }
}
