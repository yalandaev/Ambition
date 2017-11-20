using Ambition.Shared;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ambition.Bus.RabbitMQ.Transport
{
    public abstract class Consumer
    {
        protected string _exchangeName;
        protected string _queueName;
        protected readonly ISerializer _serializer;
        protected IModel _channel;
        protected IConnection _connection;
        protected EventingBasicConsumer _consumer;

        public Consumer(string exchangeName, string queueName, ISerializer serializer)
        {
            _exchangeName = exchangeName;
            _queueName = queueName;
            _serializer = serializer;
        }

        public void StartConsume()
        {
            var factory = new ConnectionFactory() { HostName = "srv-ubuntu-1", UserName = "admin", Password = "admin", VirtualHost = "DDD_Playground" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.BasicQos(0, 1, false);

            DeclareExchange();

            _consumer = new EventingBasicConsumer(_channel);

            OnRecieved();

            _channel.BasicConsume(queue: _queueName,
                autoAck: false,
                consumer: _consumer);
        }

        protected abstract void DeclareExchange();
        protected abstract void OnRecieved();
    }
}