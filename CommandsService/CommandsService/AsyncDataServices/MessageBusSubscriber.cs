using System;
using System.Text;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using CommandsService.EventProcessing;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            this._configuration = configuration;
            this._eventProcessor = eventProcessor;

            this.InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = this._configuration["RabbitMQHost"],
                Port = int.Parse(this._configuration["RabbitMQPort"])
            };

            this._connection = factory.CreateConnection();
            this._channel = this._connection.CreateModel();
            this._channel.ExchangeDeclare(
                exchange: "trigger",
                type: ExchangeType.Fanout);
            this._queueName = this._channel.QueueDeclare().QueueName;
            this._channel.QueueBind(
                queue: this._queueName,
                exchange: "trigger",
                routingKey: "");

            Console.WriteLine("--> Listening on the Message Bus...");

            this._connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(this._channel);

            consumer.Received += (moduleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received!");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                this._eventProcessor.ProcessEvent(notificationMessage);
            };

            this._channel.BasicConsume(
                queue: this._queueName,
                autoAck: true,
                consumer: consumer);

            return Task.CompletedTask;
        }

        private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }

        public override void Dispose()
        {
            if (this._channel.IsOpen)
            {
                this._channel.Close();
                this._connection.Close();
            }

            base.Dispose();
        }
    }
}
