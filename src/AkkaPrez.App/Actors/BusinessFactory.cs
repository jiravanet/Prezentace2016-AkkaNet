namespace AkkaPrez.App.Actors
{
    using Akka.Actor;

    public class BusinessFactory
    {
        public IActorRef Create(ActorSystem system)
        {
            return system.ActorOf<BusinessCoordinatorActor>("business");
        }

        public IBusiness CreateNaive()
        {
            return new BusinessActorNaive();
        }

        public IBusiness CreateLock()
        {
            return new BusinessActorLock();
        }

        public IBusiness CreateConcurrent()
        {
            return new BusinessActorConcurrent();
        }
    }
}