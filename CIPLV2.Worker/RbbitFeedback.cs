using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Worker
{
    public class RbbitFeedback : BackgroundService
    {
        private readonly IBus _busControl;
        private string commlogs = "";
        private string basepath = AppDomain.CurrentDomain.BaseDirectory;
        readonly IConfiguration _config;
        public RbbitFeedback(IBus bus, IConfiguration configuration)
        {

            _busControl = bus;

            commlogs = "C:\\Users\\mends\\AppData\\Roaming\\commlogs.txt";
            //Path.Combine(Environment.GetFolderPath(
            //   Environment.SpecialFolder.ApplicationData), "commlogs.txt");
            try
            {
                File.AppendAllText(commlogs, "Rabitt Listner Created " + basepath + "\r\n");
            }
            catch (Exception ex)
            {

                File.AppendAllText("C:\\test\\log.txt", ex.Message);
            }
           
             
            _config = configuration;
        } 
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Channel created");
            await _busControl.ReceiveFromExchangeAsync<string>("feedbackdata", x =>
            {
                Task.Run(() => {
                    File.AppendAllText(commlogs, "Message Received " + x + "\r\n");
                    MessageReceived(x);
                }, stoppingToken);
            });
        }
        private void MessageReceived(string message)
        {

            File.AppendAllText(commlogs, "In Message Received method" + message + "\r\n");
            try
            {

                if (message != null && message == System.Environment.MachineName)
                {

                    string exepath = _config["exepath"].ToString();// $"D:\\RikProjects\\CIPLV2\\CIPLV2.Ticket\\bin\\Debug\\net7.0-windows";
                    string ticketexepath = basepath.Contains("CIPLV2.Worker") ? basepath.Split("CIPLV2.Worker")[0] + exepath : basepath;
                    if (basepath.Contains("CIPLV2.Worker.exe")) ticketexepath = basepath.Split("CIPLV2.Worker.exe")[0].ToString();
                    //File.AppendAllText(commlogs, ticketexepath + "\r\n");
                    ticketexepath = "C:\\test";
                    File.AppendAllText(commlogs, "Ticket Exe Path" + ticketexepath + "\r\n");
                    var process = new Process();
                    process.StartInfo.UseShellExecute = true;
                    //process.StartInfo.WorkingDirectory = ticketexepath;
                    process.StartInfo.FileName = "C:\\test\\test.bat";
                    process.StartInfo.RedirectStandardInput = true;
                    ////process.StartInfo.FileName = ticketexepath + "\\TicketFeedBack.exe";//written by me
                    ////process.StartInfo.Arguments = message;
                    //File.AppendAllText(commlogs,"Exe path "+ process.StartInfo.FileName + "\r\n");
                    ////process.Exited += new EventHandler(process_completed);
                    
                    process.Start();
                    //process.WaitForExit();
                    File.AppendAllText(commlogs, "Process End " + process.StartInfo.FileName + "\r\n");
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(commlogs, "Error " + ex.Message + "\r\n");
                Console.WriteLine(ex.Message);
            }

        }
        
    }
}
