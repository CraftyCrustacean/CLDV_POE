using System.ComponentModel.DataAnnotations;

namespace EventEase3.Models
{
    public class EventType
    {
        public int EventTypeID { get; set; }
        public string? EventTypeName { get; set; }
        public List<Event>? Events { get; set; }
    }
}
