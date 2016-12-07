using System;

namespace AkkaPrez.App
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Actors;
    using Akka.Actor;
    using Akka.Configuration;
    using FMessages;

    class Program
    {
        static void Main(string[] args)
        {

            using (var system = CreateSystem())
            {
                var factory = new BusinessFactory();
                try
                {
                    var actor = factory.Create(system);
                    var stop = Stopwatch.StartNew();

                    Parallel.For(0, 10000000, (i) =>
                    {
                        var message = new AddMessage(i, i);
                        actor.Tell(message);
                        //actor.Add(message);
                    });
                    stop.Stop();
                    Console.WriteLine("Finished within {0}", stop.Elapsed);

                }
                catch (AggregateException ae)
                {
                    Console.WriteLine(ae.ToString());
                }
                Console.Read();
            }
        }

        static ActorSystem CreateSystem()
        {
            var system = ActorSystem.Create("akkaPrez", GetConfig());
            return system;
        }

        static Config GetConfig()
        {
            var config = ConfigurationFactory.ParseString(@"
akka.suppress-json-serializer-warning = on
");
            return config;
        }
    }
}
