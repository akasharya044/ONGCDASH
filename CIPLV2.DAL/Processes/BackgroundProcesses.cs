using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Processing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIPLV2.Models.Tickets;
using System.Net.Http.Json;
using System.Text.Json;
using CIPLV2.Models.DeviceDetail;

namespace CIPLV2.DAL.Processes
{
    public class BackgroundProcesses : BackgroundService
    {
        
        DateTime lastprocessdate = DateTime.Now;
        DateTime LastGetTickets = DateTime.Now.AddDays(1);
        int lastid = 0;
        readonly IServiceScopeFactory _serviceScopeFactory;
        readonly DeviceLogs _devicelogs;
        bool processing = false;
        int totaldevices = 0;
        int runningdevices = 0;
        private readonly IBus _busControl;
        public List<DeviceStatusPool> devicePool { get; set; } = new List<DeviceStatusPool>();
        public BackgroundProcesses(IServiceScopeFactory serviceScopeFactory,DeviceLogs deviceLogs,IBus buscontrol )
        {
            _serviceScopeFactory = serviceScopeFactory;
            _devicelogs = deviceLogs;
            //CreateQueue();
            _busControl = buscontrol;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Background worker " + DateTime.Now);

                try
                {
                    LogWriter.LogWrite("enter in try block of Executeasync");
                    if (DateTime.Now > lastprocessdate.AddMinutes(5)) await CheckStatus();
                    Console.WriteLine("Task executed at " + DateTime.Now);
                    foreach (var item in devicePool.Where(x => x.LastHeartBeat.AddMinutes(10) < DateTime.Now))
                    {
                        item.IsRunning = false;
                    }
                    //await CheckDeviceAddition();
                    

                }
                catch (Exception ex)
                {
                    LogWriter.LogWrite("enter in catch block of ExecuteAsync" + ex.Message);
                    Console.WriteLine(ex.Message);
                }
                await Task.Delay(30000, stoppingToken);
            }
        }
        public async Task CheckDeviceAddition()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorks>();
                    var lastrecord = uow.deviceDetails.GetAllNoTracking(null, x => x.OrderByDescending(x => x.Id)).FirstOrDefault();
                    if (lastrecord != null)
                    {
                        if (lastrecord.Id > lastid) CreateQueue();
                       
                    }
                }
            }
            catch (Exception ex)
            {

                
            }
              

        }
        //private async Task SendStatusToDashboard()
        //{
        //    using (var scope = _serviceScopeFactory.CreateScope())
        //    {
        //        string quename = "dashboard-data";
        //        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        //        var factory = new ConnectionFactory { HostName = config["RabbitMqHost"] };
        //        factory.UserName = "admin";
        //        factory.Password = "admin@123";
        //        using var connection = factory.CreateConnection();
        //        using var channel = connection.CreateModel();
        //       // channel.ExchangeDeclare(exchange: quename, type: ExchangeType.Fanout);
        //        channel.QueueDeclare(queue: quename,
        //                             durable: false,
        //                             exclusive: false,
        //                             autoDelete: false,
        //                             arguments: null);
        //        var data = new
        //        {
        //            TotalDevices = totaldevices,
        //            RunningDevices = runningdevices
        //        };
        //        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));

        //        channel.BasicPublish(exchange: string.Empty,
        //                             routingKey: quename,
        //                             basicProperties: null,
        //                             body: body);
        //    }
        //}
        private async Task CheckStatus()
        {
            if (DateTime.Now < lastprocessdate.AddDays(-1) || processing) return;
            lastprocessdate = DateTime.Now;
            processing = true;
            try
            {
                LogWriter.LogWrite("enter in try block of CheckStatus in background process");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorks>();
                    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    HttpClient httpClient = new HttpClient();
                    var credentials = new
                    {
                        login = config["consoleuser"],
                        password = config["consolepassword"]
                    };
                    var response = await httpClient.PostAsJsonAsync(config["ApiEndpoints:Authentication"], credentials);
                    var token = await response.Content.ReadAsStringAsync();

                    httpClient = new HttpClient();
                    var curdatetime = DateTime.Now;
                    var url = config["ApiEndpoints:GetTickets"].Replace("169281540000", (LastGetTickets.Ticks / TimeSpan.TicksPerMillisecond).ToString()).Replace("1692901799999", (curdatetime.Ticks / TimeSpan.TicksPerMillisecond).ToString());
                    //Add lwwsso token 
                    httpClient.DefaultRequestHeaders.Add("Cookie", "LWSSO_COOKIE_KEY=" + token);
                    response = await httpClient.GetAsync(url);
                    var ticketrecords = await response.Content.ReadFromJsonAsync<TicketResponseGet>();
                    foreach (var ticketrecord in ticketrecords.entities)
                    {
                        //                        LogWriter.LogWrite("Enter in foreach loop ");

                        if (ticketrecord.properties.CurrentStatus_c == "Resolved_c")
                        {
                            var tr = uow.ticketrecord.GetAll(x => x.TicketId == Convert.ToInt32(ticketrecord.properties.Id)).FirstOrDefault();
                            LogWriter.LogWrite("ticket record " + tr);
                            if (tr != null && tr.TicketStatus == 1)
                            {
                                tr.TicketStatus = 2;//resolved
                                tr.ResolvedDateTime = ticketrecord.properties.ResolvedTime_c.ToString();
                                //tr.ResolvedDateTime = Convert.ToDateTime(ticketrecord.properties.ResolvedTime_c);
                                tr.NextFeedBackSchedule = DateTime.Now;
                                tr.AssignedTo = ticketrecord.related_properties.ExpertAssignee.Name;
                                uow.ticketrecord.Update(tr);
                                uow.Save();
                            }
                        }
                    }


                    //Taking Status completed
                    uow.ClearTracker();
                    LastGetTickets = curdatetime;
                    foreach (var item in uow.ticketrecord.GetSelectedNoTracking(x => x.SystemId, x => x.TicketStatus == 2 && x.NextFeedBackSchedule <= DateTime.Now).Distinct())
                    {

                        var factory = new ConnectionFactory { HostName = config["RabbitMqHost"] };
                        factory.UserName = "admin";
                        factory.Password = "admin@123";
                        using var connection = factory.CreateConnection();
                        using var channel = connection.CreateModel();

                        channel.QueueDeclare(queue: item,
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: true,
                                             arguments: null);

                        var body = Encoding.UTF8.GetBytes("Feedback Requested");

                        channel.BasicPublish(exchange: string.Empty,
                                             routingKey: item,
                                             basicProperties: null,
                                             body: body);


                    };
                    processing = false;
                }

            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Check Status Exception.");
                LogWriter.LogWrite(ex.ToString());
                processing = false;

            }

        }
        public void CreateQueue()
        {
            Task.Run(async () =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    //devicePool = new List<DeviceStatusPool>();
                    _devicelogs.Init();
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorks>();
                    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    foreach (var device in uow.deviceDetails.GetSelectedNoTracking(x => x.DisplayLabel, x => (x.SubType == "Laptop" || x.SubType == "Desktop") && x.IsDeleted == false).Distinct())
                    {
                        _devicelogs.AddDevice(new DeviceStatusPool { DeviceId = device,IsRunning=false, LastHeartBeat = DateTime.Now.AddDays(-1) });
                       

                    }

                    totaldevices = devicePool.Count;
                    var factory = new ConnectionFactory { HostName = config["RabbitMqHost"] };
                    factory.UserName = "admin";
                    factory.Password = "admin@123";
                    using var connection = factory.CreateConnection();
                    using var channel = connection.CreateModel();

                    channel.QueueDeclare(queue: "devices-HB",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: true,
                                         arguments: null);



                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        OnMessageRecived(message);
                    };
                    channel.BasicConsume(queue: "devices-HB",
                                         autoAck: true,
                                         consumer: consumer);
                   
                    var lastrecord = uow.deviceDetails.GetAllNoTracking(null,x=>x.OrderByDescending(x=>x.Id)).FirstOrDefault();
                    if (lastrecord != null) { lastid= lastrecord.Id; }
                }

            });

        }
        private void OnMessageRecived(string message)
        {
            try
            {
//                LogWriter.LogWrite("enter in try block of OnMessageRecived");
                _devicelogs.UpdateDevice(message); 
                
            }
            catch (Exception ex)
            {
//                LogWriter.LogWrite("Error in onmessagerecieved" + ex.Message);
               
            }
        }
    }
}
