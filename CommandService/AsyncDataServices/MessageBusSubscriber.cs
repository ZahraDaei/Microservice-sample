using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CommandService.Dtos;
using CommandService.EventProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private  IConnection _connection;
        private  IModel _channel;
        private  IEventProcessor _eventProcessor;
        private string _queueName;
        private readonly IConfiguration _configuration;
        //  private readonly EventingBasicConsumer _consumer;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _eventProcessor = eventProcessor;
            _configuration = configuration;
            InitializeRabbitMQ();

         

        }

        private void InitializeRabbitMQ()
        {
            try
            {
                
           
            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName,
                exchange: "trigger",
                routingKey: "");

            Console.WriteLine("--> Listenting on the Message Bus...");

             }
            catch (System.Exception)
            {
                
                Console.WriteLine("-->Can not connect to message bus");
            }
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            

            Console.WriteLine("-->shutdown connection ");


        }

          public override void Dispose()
        {
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose(); 
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var _consumer = new EventingBasicConsumer(_channel);

            string consumerTag = _channel.BasicConsume("", false, _consumer);

            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"-->messge:{message}");
                _eventProcessor.ProcessEvent(message);

            };
            return Task.CompletedTask;

        }
    }
}