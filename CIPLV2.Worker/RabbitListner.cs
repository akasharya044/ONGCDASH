using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Worker
{
    public class RabbitListner
    {
        private string commlogs = "";
        private string basepath = AppDomain.CurrentDomain.BaseDirectory;
        readonly IConfiguration _config;
        public RabbitListner(IBus rabbitbus,IConfiguration configuration)
        {
             commlogs = Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.ApplicationData), "commlogs.txt");
            File.AppendAllText(commlogs, "Rabitt Listner Created " + basepath +"\r\n");
            rabbitbus.ReceiveFromExchangeAsync<string>("feedbackdata", x => OnMessageReceived(x));
            _config = configuration;
        }
        private void OnMessageReceived(string message)
        {
            File.AppendAllText(commlogs, "Message Received " + message + "\r\n");
            try
            {
                
                if (message != null && message == System.Environment.MachineName)
                {

                    string exepath = _config["exepath"].ToString();// $"D:\\RikProjects\\CIPLV2\\CIPLV2.Ticket\\bin\\Debug\\net7.0-windows";
                    string ticketexepath = basepath.Contains("CIPLV2.Worker") ? basepath.Split("CIPLV2.Worker")[0] + exepath : basepath;
                    if (basepath.Contains("CIPLV2.Worker.exe")) ticketexepath = basepath.Split("CIPLV2.Worker.exe")[0].ToString();
                    File.AppendAllText(commlogs, ticketexepath + "\r\n");
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
    }
}
