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
        private ISerializer _serializer;

        public static Bus Instance { get; private set; }

        public static void Initialize(IBusTransport transport, ISerializer serializer)
        {
            Bus bus = new Bus();
            bus._transport = transport;
            bus._serializer = serializer;
            //bus.consumerTypes = new Dictionary<string, Type>();

            Instance = bus;
        }


        private Bus()
        {
        }

        public void Send(ICommand command)
        {
            Console.WriteLine($"Send {command.GetType()} command");
            _transport.SendMessage("Commands", command.GetType().ToString(), _serializer.Serialize(command));
        }

        public void Publish(IEvent @event)
        {
            Console.WriteLine($"Publish {@event.GetType()} event");
            _transport.SendMessage("Events", @event.GetType().ToString(), _serializer.Serialize(@event));
        }

        //public static void Initialize(Action<IBusConfigurator> action)
        //{
        //    action(new BusConfigurator());
        //}

        public void SubscribeCommandHandler<TCommand, THandler>() where TCommand : ICommand
        {
            int consumersCount = 1;
            _transport.Subscribe<TCommand, THandler>(exchangeName: "Commands",
                                                     queueName: typeof(TCommand).ToString(),
                                                     consumersCount: consumersCount);
        }

        public void SubscribeEventHandler<TEvent, THandler>() where TEvent : IEvent
        {
            int consumersCount = 1;
            _transport.Subscribe<TEvent, THandler>(exchangeName: "Events",
                                                   queueName: typeof(THandler).ToString(),
                                                   consumersCount: consumersCount);
        }

        #region Sagas

        //private Dictionary<string, Type> consumerTypes;

        //public void RegisterSaga(Type sagaType)
        //{
        //    //Получить все типы сообщений, который обрабатывает
        //    var types = sagaType.GetInterfaces();

        //    foreach (Type type in types)
        //    {
        //        var baseType = type.GenericTypeArguments[0].GetInterfaces()[0];
        //        consumerTypes.Add(type.ToString(), type.GenericTypeArguments[0]);
        //    }
        //}

        #endregion


    }
}