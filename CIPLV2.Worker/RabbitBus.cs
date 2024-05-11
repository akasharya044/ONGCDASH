using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CIPLV2.Worker
{
    public class RabbitBus : IBus
    {
        private readonly IModel _channel;
        private string commlogs = "";
        internal RabbitBus(IModel channel)
        {
            _channel = channel;
            commlogs = "C:\\Users\\mends\\AppData\\Roaming\\commlogs.txt";
            //commlogs = Path.Combine(Environment.GetFolderPath(
            //                Environment.SpecialFolder.ApplicationData), "commlogs.txt");
        }
        public async Task SendAsync<T>(string queue, T message)
        {
            await Task.Run(() =>
            {
                _channel.QueueDeclare(queue, true, false, false);
                var properties = _channel.CreateBasicProperties();
                properties.Persistent = false;
                var output = JsonConvert.SerializeObject(message);
                _channel.BasicPublish(string.Empty, queue, null,
                Encoding.UTF8.GetBytes(output));
            });
        }
        public async Task ReceiveAsync<T>(string queue, Action<string> onMessage)
        {
            _channel.QueueDeclare(queue, true, false, false);
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (s, e) =>
            {
                Console.WriteLine("Message Received ");
                byte[] body = e.Body.ToArray(); 
                var message = Encoding.UTF8.GetString(body); 
                
                onMessage(message);
                await Task.Yield();
            };
            _channel.BasicConsume(queue, true, consumer);
            await Task.Yield();
        }
        public async Task ReceiveFromExchangeAsync<T>(string exchangename, Action<string> onMessage)
        {

            _channel.ExchangeDeclare(exchange: exchangename, type: ExchangeType.Fanout);

            // declare a server-named queue
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                              exchange: exchangename,
                              routingKey: string.Empty);


            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (s, e) =>
            { 
               
                byte[] body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                File.AppendAllText(commlogs, "Message Received from Rabbitmq " + message + "\r\n");
                onMessage(message);
                await Task.Yield();
            };

            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

    
            await Task.Yield();
        }
    }
}
