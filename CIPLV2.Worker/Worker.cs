using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;
using System.Management;
using CIPLV2.Models.DeviceDetail;
using System.Net.Http.Json;
using CIPLV2.Models.Admin;
using System.Threading.Channels;
using System.Linq;

namespace CIPLV2.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private string basepath = AppDomain.CurrentDomain.BaseDirectory;
        private string  logpath = "";
        private string errorlogs = "";
        private readonly IConfiguration _config;
        private readonly IBus _buscontrol;
 
        public Worker(ILogger<Worker> logger, IConfiguration config,IBus bus,RabbitListner rabbitListner)
        {
            _logger = logger;
            _config = config;
             logpath = Path.Combine(Environment.GetFolderPath(
      Environment.SpecialFolder.ApplicationData), "devicelogs.txt");
            errorlogs = Path.Combine(Environment.GetFolderPath(
Environment.SpecialFolder.ApplicationData), "errolog.txt");
            _buscontrol = bus;
             
            //RabbitListner(); 
            CheckLog();
            RegisterDevice();
        }
        public void RegisterDevice()
        {
            Task.Factory.StartNew(async () =>
            {
                try
            {
                DeviceDetailsDto deviceDetailsDto = new DeviceDetailsDto();
                deviceDetailsDto.entity_type = "Device";
                deviceDetailsDto.properties.IsDeleted = false;
                deviceDetailsDto.properties.DisplayLabel = System.Environment.MachineName;
                deviceDetailsDto.properties.LastUpdateTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                deviceDetailsDto.properties.SubType = "Desktop";
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_config["ApiUrl"]);
                
                var response = await httpClient.PostAsJsonAsync(_config["ApiEndpoints:RegisterDevice"], deviceDetailsDto);
                var data = await response.Content.ReadFromJsonAsync<Response>();
                string test = "T";


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                 
            }
            }, CancellationToken.None);
        }
        private void CheckLog()
        {
            
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    var log = System.IO.File.ReadAllText(logpath);
                    if (log != null)
                    {
                        //HttpClient httpClient = new HttpClient();
                        HttpClient httpClient = new HttpClient();
                        httpClient.BaseAddress = new Uri(_config["ApiUrl"]);
                        DeviceRunningLogDTO deviceRunningLogDTO = new DeviceRunningLogDTO
                        {
                            SystemId = System.Environment.MachineName,
                            StartTime = Convert.ToDateTime(log.Split('-')[0]),
                            ShutDownTime = Convert.ToDateTime(log.Split('-')[1])
                        };
                        var response = await httpClient.PostAsJsonAsync(_config["ApiEndpoints:AddLog"], deviceRunningLogDTO);
                        var data = await response.Content.ReadFromJsonAsync<Response>();
                        string test = "T";
                    }
                    System.IO.File.WriteAllText(logpath, DateTime.Now.ToString() + "-" + DateTime.Now.ToString());
                    
                }
                catch (Exception ex)
                {

                    System.IO.File.WriteAllText(logpath, DateTime.Now.ToString() + "-" + DateTime.Now.ToString());
                     
                }

            }, CancellationToken.None);
               

            
           
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //await _buscontrol.ReceiveFromExchangeAsync<string>("feedbackdata", x =>
            //{
            //    Task.Run(() =>
            //    {
            //        Console.Write("Message received " + x);
            //        OnMessageReceived(x);
            //    }, stoppingToken);
            //});
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
              

                UpdateLog();
                await Task.Delay(30000, stoppingToken);
            }
          
        }
        private void UpdateLog()
        {
            try
            {
                var log = System.IO.File.ReadAllText(logpath);
                if (log != null)
                {
                    log = log.Split('-')[0]+"-"+DateTime.Now.ToString();
                }
                System.IO.File.WriteAllText(logpath,log);
                SendHeartBeat();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                System.IO.File.WriteAllText(logpath, DateTime.Now.ToString() + "-" + DateTime.Now.ToString());
            }
        }
        private void SendHeartBeat()
        {
            
            var factory = new ConnectionFactory { HostName = _config["RabbitMqHost"] };
            factory.UserName = "admin";
            factory.Password = "admin@123";
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

           
            channel.QueueDeclare("deviceshb", true, false, false);

            var body = Encoding.UTF8.GetBytes(System.Environment.MachineName);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "deviceshb",
                                 basicProperties: null,
                                 body: body);
           

            
        }
        private void RabbitListner()
        {
            string srno = System.Environment.MachineName;


            var factory = new ConnectionFactory { HostName = _config["RabbitMqHost"] };
            factory.UserName = "admin";
            factory.Password = "admin@123";
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: srno,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: true,
                                 arguments: null);

           Console.WriteLine(" [*] Waiting for messages. on deviceid " + srno);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                OnMessageReceived(message);
                //Console.WriteLine($" [x] Received {message}");
            };
            channel.BasicConsume(queue: srno,
                                 autoAck: true,
                                 consumer: consumer);


          

        }
        private void OnMessageReceived(string message)
        {
           
            try
            {
                File.AppendAllText(errorlogs, basepath+"\r\n");
                if (message != null && message == System.Environment.MachineName)
                {

                    string exepath = _config["exepath"].ToString();// $"D:\\RikProjects\\CIPLV2\\CIPLV2.Ticket\\bin\\Debug\\net7.0-windows";
                    string ticketexepath = basepath.Contains("CIPLV2.Worker") ? basepath.Split("CIPLV2.Worker")[0] + exepath : basepath;
                    if (basepath.Contains("CIPLV2.Worker.exe")) ticketexepath = basepath.Split("CIPLV2.Worker.exe")[0].ToString();
                    File.AppendAllText(errorlogs, ticketexepath+"\r\n");
                    var process = new Process();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.WorkingDirectory = ticketexepath;
                    process.StartInfo.FileName = ticketexepath + "\\CIPLV2.Ticket.exe";
                    //process.StartInfo.FileName = ticketexepath + "\\TicketFeedBack.exe";//written by me
                    process.StartInfo.Arguments = message;

                    //process.Exited += new EventHandler(process_completed);
                    process.Start();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            
           
        }
        //public static string GetMACAddress()
        //{
        //    ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
        //    ManagementObjectCollection moc = mc.GetInstances();
        //    string MACAddress = String.Empty;
        //    foreach (ManagementObject mo in moc)
        //    {
        //        if (MACAddress == String.Empty)
        //        {
        //            if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
        //        }
        //        mo.Dispose();
        //    }

        //    MACAddress = MACAddress.Replace(":", "");
        //    return MACAddress;
        //}
    }
}