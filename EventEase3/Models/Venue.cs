using System.ComponentModel.DataAnnotations;

namespace EventEase3.Models
{
    public class Venue
    {
        public int VenueId { get; set; }
        [Display(Name = "Venue Name")]
        public string? VenueName { get; set; }
        [Display(Name = "Venue Location")]
        public string? VenueLocal { get; set; }
        [Display(Name = "Venue Capacity")]
        public int VenueCap { get; set; }
        [Display(Name = "Venue Image URL")]
        public string? VenueImgURL { get; set; }
        public List<Booking>? Bookings { get; set; }
        public bool Availability { get; set; }

    }
}
