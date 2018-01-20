using System;
using System.Text;
using System.Windows.Input;
using Ambition.Shared;
using RabbitMQ.Client;

namespace Ambition.Bus.RabbitMQ.Transport
{
    /// <summary>
    /// IBusTransport implementation using RabbitMQ
    /// </summary>
    public class RabbitMqBusTransport : IBusTransport
    {
        protected readonly ISerializer _serializer;

        public RabbitMqBusTransport(ISerializer serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// Send command message to queue
        /// </summary>
        /// <param name="command">Command</param>
        public void SendCommand(ICommand command)
        {
            SendMessage("Commands", command.GetType().ToString(), _serializer.Serialize(command));
        }

        /// <summary>
        /// Send event message to queue
        /// </summary>
        /// <param name="event">Event</param>
        public void PublishEvent(IEvent @event)
        {
            SendMessage("Events", @event.GetType().ToString(), _serializer.Serialize(@event));
        }

        /// <summary>
        /// Subscribe handler
        /// </summary>
        /// <typeparam name="TType">Message type</typeparam>
        /// <typeparam name="THandler">Message handler type</typeparam>
        public void Subscribe<TType, THandler>()
        {
            Console.WriteLine($"Subscribe { typeof(TType) } to handler { typeof(THandler) }");

            if (typeof(IEvent).IsAssignableFrom(typeof(TType)))
            {
                Subscribe<TType, THandler>(exchangeName: "Events", queueName: typeof(THandler).ToString());
            }
            if (typeof(ICommand).IsAssignableFrom(typeof(TType)))
            {
                Subscribe<TType, THandler>(exchangeName: "Commands", queueName: typeof(TType).ToString());
            }
        }

        /// <summary>
        /// Subscribe message handler to queue
        /// </summary>
        /// <typeparam name="TType">Message type</typeparam>
        /// <typeparam name="THandler">Message handler type</typeparam>
        /// <param name="exchangeName">Exchange name</param>
        /// <param name="queueName">Queue name</param>
        private void Subscribe<TType, THandler>(string exchangeName, string queueName)
        {
            Consumer consumer = new BusConsumer<TType, THandler>(exchangeName, queueName, _serializer);
            consumer.StartConsume();
        }

        /// <summary>
        /// Enqueue message
        /// </summary>
        /// <param name="exchangeName">Exchange name</param>
        /// <param name="routingKey">Routing key</param>
        /// <param name="message">Message to send</param>
        private void SendMessage(string exchangeName, string routingKey, string message)
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