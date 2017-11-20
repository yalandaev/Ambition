using System;
using System.Text;
using System.Windows.Input;
using Ambition.Shared;
using RabbitMQ.Client;

namespace Ambition.Bus.RabbitMQ.Transport
{
    public class RabbitMqBusTransport : IBusTransport
    {
        protected readonly ISerializer _serializer;

        public RabbitMqBusTransport(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public void Subscribe<TType, THandler>(string exchangeName, string queueName, int consumersCount)
        {
            Console.WriteLine($"Subscribe { typeof(TType) } to handler { typeof(THandler) }");
            if (typeof(IEvent).IsAssignableFrom(typeof(TType)))
            {
                for (int i = 0; i < consumersCount; i++)
                {
                    Consumer consumer = new EventConsumer<TType, THandler>(exchangeName, queueName, _serializer);
                    consumer.StartConsume();
                }
            }
            if (typeof(ICommand).IsAssignableFrom(typeof(TType)))
            {
                for (int i = 0; i < consumersCount; i++)
                {
                    Consumer consumer = new CommandConsumer<TType, THandler>(exchangeName, queueName, _serializer);
                    consumer.StartConsume();
                }
            }
        }

        public void SendMessage(string exchangeName, string routingKey, string message)
        {
            Console.WriteLine($"Send message: { message }");

            var factory = new ConnectionFactory() { HostName = "srv-ubuntu-1", UserName = "admin", Password = "admin", VirtualHost = "DDD_Playground" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: exchangeName,
                    routingKey: routingKey,
                    basicProperties: null,
                    body: body);
            }
        }
    }
}