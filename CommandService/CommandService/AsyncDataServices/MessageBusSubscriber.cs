
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IConfiguration _configuration; // field to retrieve configuration settings
        private IConnection _connection; // field to manage the connection to RabbitMQ
        private IModel _channel; // field to manage the channel for communication with RabbitMQ
        private string _queueName; // field to store the name of the queue

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _eventProcessor = eventProcessor;
            _configuration = configuration;
            InitRabitMQ();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested(); // check if cancellation is requested

            var consumer = new EventingBasicConsumer(_channel); // create a new consumer for the channel

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("Event Received!");
                var body = ea.Body.ToArray(); // get the message body
                var notificationMessage = Encoding.UTF8.GetString(body); // decode the message body

                _eventProcessor.ProcessEvent(notificationMessage); // process the event
            };

            _channel.BasicConsume(
                queue: _queueName,
                autoAck: true, // automatically acknowledge the message
                consumer: consumer // use the created consumer
                );

            return Task.CompletedTask; // complete the task

        }

        private void InitRabitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            }; // create a new connection factory with the specified host and port

            _connection = factory.CreateConnection(); // create a new connection using the factory
            _channel = _connection.CreateModel(); // create a new channel using the connection
            _channel.ExchangeDeclare(
                exchange: "trigger",
                type: ExchangeType.Fanout
                ); // declare an exchange of type Fanout for broadcasting messages
            _queueName = _channel.QueueDeclare().QueueName; // declare a new queue and get its name
            _channel.QueueBind(
                queue: _queueName,
                exchange: "trigger",
                routingKey: ""
                ); // bind the queue to the exchange with an empty routing key
            Console.WriteLine("--> Listening on the Message Bus..."); // log that we are listening on the message bus
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown; // subscribe to the connection shutdown event
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown"); // log that the connection has been shut down 
        }

        public override void Dispose()
        {
            Console.WriteLine("--> Disposing RabbitMQ");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
    }
}
