using System.ComponentModel.DataAnnotations;

namespace EventEase3.Models
{
    public class Booking
    {
        [Display(Name = "Booking ID")]
        public int BookingId { get; set; }
        [Display(Name = "Venue ID")]
        public int VenueId { get; set; }
        [Display(Name = "Event ID")]
        public int EventId { get; set; }
        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }
        public Venue? Venue { get; set; }
        public Event? Event { get; set; }
    }
}
