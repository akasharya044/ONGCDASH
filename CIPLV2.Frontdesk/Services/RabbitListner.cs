
using CIPLV2.Frontdesk.Components.Dto;
using Newtonsoft.Json;

namespace CIPLV2.Frontdesk.Services
{
    public class RabbitListner : BackgroundService
    {
    
        private readonly IBus _busControl;
        public RabbitListner( IBus bus)
        {
         
            _busControl = bus;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Channel created");
            await _busControl.ReceiveFromExchangeAsync<string>("dashboarddata", x =>
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
               SessionState.Device = JsonConvert.DeserializeObject<DeviceDto>(message);
               Console.WriteLine("Current Count " + SessionState.Device.TotalDevices);
                
            }
            catch (Exception ex)
            {

                
            }

        }
         
    }
}
