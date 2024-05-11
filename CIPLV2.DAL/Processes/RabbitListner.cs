using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.Processes
{
    public class RabbitListner : BackgroundService
    {
        readonly DeviceLogs _devicelogs;
        private readonly IBus _busControl;
        public RabbitListner(DeviceLogs deviceLogs,IBus bus)
        {
            _devicelogs = deviceLogs;
            _busControl = bus;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Channel created");
            await _busControl.ReceiveAsync<string>("deviceshb", x =>
            {
                Task.Run(() => { 
                    Console.Write("Message received " + x); 
                    MessageReceived(x);
                    }, stoppingToken);
            });
        }
        private void MessageReceived(string message)
        {
            try
            {
                _devicelogs.UpdateDevice(message);
            }
            catch (Exception ex)
            {

                LogWriter.LogWrite("Error in message received " + ex.Message);
            }
            
        }
    }
}
