using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Frontdesk.Services
{
    public interface IBus
    {
        Task SendAsync<T>(string queue, T message);
        Task ReceiveAsync<T>(string queue, Action<string> onMessage);
        Task ReceiveFromExchangeAsync<T>(string exchangename, Action<string> onMessage);
    }
}
