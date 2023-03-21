using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SpecialOffers.Events;

[ApiController]
[Route("/events")]
public class EventFeedController : ControllerBase
{
    private readonly IEventStore _eventStore;
    public EventFeedController(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    [HttpGet("")]
    public ActionResult<EventFeedEvent[]> GetEvents([FromQuery] long start, [FromQuery] long end = long.MaxValue)
    {
        if(start < 0 || start > end)
        {
            return BadRequest();
        }

        return Ok(_eventStore.GetEvents(start, end).ToArray());
    }
}
