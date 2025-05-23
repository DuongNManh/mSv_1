﻿using PlatformService.Models.EventMessages;
using PlatformService.Models.Requests;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQHost"],
                Port = int.Parse(_config["RabbitMQPort"])
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            Console.WriteLine("--> Connected to Message Bus");
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public void PublishNewPlatform(PlatformPublishRequest plat)
        {
            var eventMessage = EventMessageResponseBuilder.Build("Platform_Published", plat);
            var message = JsonSerializer.Serialize(eventMessage);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                // send message
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection is closed, not sending");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body
                );
            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("--> MessageBus disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }

}