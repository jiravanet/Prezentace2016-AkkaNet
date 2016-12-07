namespace AkkaPrez.App.Actors
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Akka.Actor;
    using Calc;
    using FMessages;

    public class BusinessActorNaive : IBusiness
    {
        readonly Dictionary<int, int> storage;

        public BusinessActorNaive()
        {
            storage = new Dictionary<int, int>();
        }

        public void Add(AddMessage message)
        {
            int controlDigit = ControlDigitAlgorithms.ForMSFest.GetControlDigit(message.Value);

            storage.Add(message.Key, message.Value);
        }
    }

































    public class BusinessActorLock : IBusiness
    {
        readonly Dictionary<int, int> storage;
        static readonly object @lock = new object();

        public BusinessActorLock()
        {
            storage = new Dictionary<int, int>();
        }

        public void Add(AddMessage message)
        {
            int controlDigit = ControlDigitAlgorithms.ForMSFest.GetControlDigit(message.Value);
            lock (@lock)
            {
                storage.Add(message.Key, message.Value);
            }
        }
    }








































    public class BusinessActorConcurrent : IBusiness
    {
        readonly ConcurrentDictionary<int, int> storage;

        public BusinessActorConcurrent()
        {
            storage = new ConcurrentDictionary<int, int>();
        }

        public void Add(AddMessage message)
        {
            int controlDigit = ControlDigitAlgorithms.ForMSFest.GetControlDigit(message.Value);
            storage.AddOrUpdate(message.Key, message.Value, (key, value) => value);
        }
    }







































    public class BusinessActor : ReceiveActor
    {
        readonly Dictionary<int, int> storage;
        public BusinessActor()
        {
            storage = new Dictionary<int, int>();

            Receive<AddMessage>(message => { Add(message); });

        }



        void Add(AddMessage message)
        {
            int controlDigit = ControlDigitAlgorithms.ForMSFest.GetControlDigit(message.Value);
            storage.Add(message.Key, message.Value);

        }

    }































    public class BusinessCoordinatorActor : ReceiveActor
    {
        readonly Dictionary<int, IActorRef> actors;
        public BusinessCoordinatorActor()
        {
            actors = new Dictionary<int, IActorRef>();
            Receive<AddMessage>(message =>
            {
                if (!actors.ContainsKey(message.Key%4))
                {
                    var a = Context.ActorOf(Props.Create<BusinessActor>());
                    actors[message.Key % 4] = a;
                }
                var actor = actors[message.Key % 4];
                actor.Tell(message);
            });
        }
    }

}