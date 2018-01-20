using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using Ambition.Shared;

namespace Ambition.Bus
{
    public class Bus
    {
        private IBusTransport _transport;

        public static Bus Instance { get; private set; }

        public static void Initialize(IBusTransport transport)
        {
            Bus bus = new Bus();
            bus._transport = transport;

            Instance = bus;
        }


        private Bus()
        {
        }

        public void Send(ICommand command)
        {
            Console.WriteLine($"Send {command.GetType()} command");
            _transport.SendCommand(command);
        }

        public void Publish(IEvent @event)
        {
            Console.WriteLine($"Publish {@event.GetType()} event");
            _transport.PublishEvent(@event);
        }

        public void SubscribeCommandHandler<TCommand, THandler>() where TCommand : ICommand
        {
            _transport.Subscribe<TCommand, THandler>();
        }

        public void SubscribeEventHandler<TEvent, THandler>() where TEvent : IEvent
        {
            _transport.Subscribe<TEvent, THandler>();
        }
    }
}