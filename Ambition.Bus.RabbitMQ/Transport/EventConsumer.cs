﻿using System;
using System.Reflection;
using System.Text;
using Ambition.Shared;
using RabbitMQ.Client;

namespace Ambition.Bus.RabbitMQ.Transport
{
    public class EventConsumer<TEvent, THandler> : Consumer
    {
        private THandler _handler;

        public EventConsumer(string exchangeName, string queueName, ISerializer serializer) : base(exchangeName, queueName, serializer)
        {
            _handler = (THandler)Activator.CreateInstance(typeof(THandler));
        }

        protected override void DeclareExchange()
        {
            string routingKey = typeof(TEvent).ToString();
            _channel.ExchangeDeclare(exchange: _exchangeName, type: "topic");
            _channel.QueueDeclare(queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueBind(queue: _queueName,
                exchange: _exchangeName,
                routingKey: routingKey);
        }

        protected override void OnRecieved()
        {
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                TEvent command = _serializer.Deserialize<TEvent>(message);

                MethodInfo methodInfo = _handler.GetType().GetMethod("Handle");
                object[] parametersArray = new object[] { command };
                methodInfo.Invoke(_handler, parametersArray);



                _channel.BasicAck(ea.DeliveryTag, false);
            };
        }
    }
}