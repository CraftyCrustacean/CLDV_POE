using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase3.Data;
using EventEase3.Models;
using System.Diagnostics;

namespace EventEase3.Controllers
{
    public class BookingsController : Controller
    {
        private readonly EventEase3Context _context;

        public BookingsController(EventEase3Context context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(string searchString, DateTime? fromDate, DateTime? toDate, string eventTypeFilter, bool availabilityFilter)
        {
            var bookings = _context.Booking.Include(b => b.Event).ThenInclude(b => b.EventType).Include(b => b.Venue).AsQueryable(); ;

            if (!string.IsNullOrEmpty(searchString))
            {
                // Try parsing the search string as a Booking ID
                if (int.TryParse(searchString, out int bookingId))
                {
                    bookings = bookings.Where(b => b.BookingId == bookingId ||
                                                   b.Event.EventName.ToUpper().Contains(searchString.ToUpper()));
                }
                else
                {
                    bookings = bookings.Where(b => b.Event.EventName.ToUpper().Contains(searchString.ToUpper()));
                }

            }

            // Filter by date range
            if (fromDate.HasValue)
            {
                bookings = bookings.Where(b => b.BookingDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                bookings = bookings.Where(b => b.BookingDate <= toDate.Value);
            }

            // Filter by Event Type
            if (!string.IsNullOrEmpty(eventTypeFilter))
            {
                bookings = bookings.Where(b => b.Event.EventType.EventTypeName == eventTypeFilter);
            }

            if (availabilityFilter)
            {
                // Get bookings where the venue is available
                bookings = bookings.Where(b => b.Venue.Availability == true);
            }

            ViewBag.EventTypeList = new SelectList(await _context.EventType.Select(et => et.EventTypeName).Distinct().ToListAsync());

            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewBag.EventId = new SelectList(
                _context.Event
                    .Select(e => new { e.EventId, Display = e.EventId + " - " + e.EventName }),
                "EventId", "Display"           
            );

            ViewBag.VenueId = new SelectList(
                _context.Venue
                    .Select(v => new { v.VenueId, Display = v.VenueId + " - " + v.VenueName }),
                "VenueId", "Display"
            ); 
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,VenueId,EventId,BookingDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Check if a booking already exists for this venue on this date
                bool isDoubleBooking = await _context.Booking
                    .AnyAsync(b => b.VenueId == booking.VenueId && b.BookingDate.Date == booking.BookingDate.Date);

                if (isDoubleBooking)
                {
                    ModelState.AddModelError("", "This venue is already booked on the selected date.");
                }
                else
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }            
            
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueId", booking.VenueId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.EventId = new SelectList(
                _context.Event
                    .Select(e => new { e.EventId, Display = e.EventId + " - " + e.EventName }),
                "EventId", "Display"
            );

            ViewBag.VenueId = new SelectList(
                _context.Venue
                    .Select(v => new { v.VenueId, Display = v.VenueId + " - " + v.VenueName }),
                "VenueId", "Display"
            );
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,VenueId,EventId,BookingDate")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check for conflicting bookings, excluding the current one
                bool isDoubleBooking = await _context.Booking
                    .AnyAsync(b => b.VenueId == booking.VenueId
                                && b.BookingDate.Date == booking.BookingDate.Date
                                && b.BookingId != booking.BookingId); // Exclude current booking

                if (isDoubleBooking)
                {
                    ModelState.AddModelError("", "This venue is already booked on the selected date.");
                }
                else
                {
                    try
                    {
                        _context.Update(booking);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }

                    catch (DbUpdateConcurrencyException)

                    {
                        if (!BookingExists(booking.BookingId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueId", booking.VenueId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
    }
}
