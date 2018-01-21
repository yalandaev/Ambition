using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambition.Bus;
using Ambition.Bus.RabbitMQ.Transport;
using Ambition.Shared;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IBusTransport _transport = new RabbitMqBusTransport(new JsonSerializer());
            Bus.Initialize(_transport);
            Bus.Instance.SubscribeCommandHandler<SampleCommand, SampleCommandHandler>();
            Bus.Instance.SubscribeCommandHandler<SampleCommand, SampleCommandHandler>();

            Bus.Instance.SubscribeEventHandler<SampleEvent, SampleEventHandler>();
            Bus.Instance.SubscribeEventHandler<SampleEvent, SampleEventHandler>();
            Bus.Instance.SubscribeEventHandler<SampleEvent, AnotherSampleEventHandler>();

            Bus.Instance.Send(new SampleCommand(){ Name = "Hello Command!" });
        }
    }

    public class SampleCommand: ICommand
    {
        public string Name { get; set; }
    }

    public class SampleCommandHandler: ICommandHandler<SampleCommand>
    {
        public void Handle(SampleCommand command)
        {
            Console.WriteLine($"Handle command {command.Name}");
            Bus.Instance.Publish(new SampleEvent() { Name = "Hello Event!" });
        }
    }

    public class SampleEvent : IEvent
    {
        public string Name { get; set; }
    }

    public class SampleEventHandler : IEventHandler<SampleEvent>
    {
        public void Handle(SampleEvent @event)
        {
            Console.WriteLine($"Handle Event {@event.Name}");
        }
    }

    public class AnotherSampleEventHandler : IEventHandler<SampleEvent>
    {
        public void Handle(SampleEvent @event)
        {
            Console.WriteLine($"Handle Event {@event.Name}");
        }
    }
}
