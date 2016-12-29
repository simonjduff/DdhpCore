using System.Collections.Generic;
using DdhpCore.EventSource.Events;
using DdhpCore.Storage;
using NSubstitute;

namespace DdhpCore.EventSource.Tests
{
    public abstract class EventSourceTestBase
    {
        protected EventEntityFactory EventEntityFactory;
        private IStorageFacade _storageFacade;
        protected int EntityVersion = 0;
        protected readonly List<Event> Events = new List<Event>();

        protected void GivenAnEntityFactory()
        {
            _storageFacade = Substitute.For<IStorageFacade>();
            _storageFacade.GetAllByPartition<Event>(Arg.Any<string>(), Arg.Any<string>()).Returns(Events);
            EventEntityFactory = new EventEntityFactory(_storageFacade);
        }
    }
}