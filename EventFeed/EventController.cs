using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.EventFeed
{

    [Route("/events")]
    public class EventFeedController(IEventStore eventStore) : Controller
    {
        [HttpGet("")]
        public Event[] Get([FromQuery] long start, [FromQuery] long end = long.MaxValue)
        {
            return eventStore
                .GetEvents(start, end)
                .ToArray();
        }
    }
}