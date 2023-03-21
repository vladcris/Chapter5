using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecialOffers.Events;

public interface IEventStore
{
    IEnumerable<EventFeedEvent> GetEvents(long start, long end);
    void RaiseEvent(string name, object content);
}

public class EventStore : IEventStore
{
    private static long currentSequenceNumber = 0;
    private static readonly IList<EventFeedEvent> Database = new List<EventFeedEvent>();
    public IEnumerable<EventFeedEvent> GetEvents(long start, long end)
    {
        var query = Database.Where(e => e.SequenceNumber >= start && e.SequenceNumber <= end)
            .OrderBy(e => e.SequenceNumber);
        return query;
    }

    public void RaiseEvent(string name, object content)
    {
        var seqNumber = Interlocked.Increment(ref currentSequenceNumber);
        Database.Add(new EventFeedEvent
        (
            seqNumber,
            DateTimeOffset.UtcNow,
            name,
            content
        ));

    }
}
