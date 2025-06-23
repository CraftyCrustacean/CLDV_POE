using System.ComponentModel.DataAnnotations;

namespace EventEase3.Models
{
    public class Event
    {
        public int EventId { get; set; }
        [Display(Name = "Event Name")]
        public string? EventName { get; set; }
        [Display(Name = "Event Description")]
        public string? EventDesc { get; set; }
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }
        public List<Booking>? Bookings { get; set; }
        public int EventTypeID { get; set; }
        public EventType? EventType { get; set; }
    }
}
