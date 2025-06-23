using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventEase3.Models;

namespace EventEase3.Data
{
    public class EventEase3Context : DbContext
    {
        public EventEase3Context (DbContextOptions<EventEase3Context> options)
            : base(options)
        {
        }

        public DbSet<EventEase3.Models.Venue> Venue { get; set; } = default!;
        public DbSet<EventEase3.Models.Event> Event { get; set; } = default!;
        public DbSet<EventEase3.Models.Booking> Booking { get; set; } = default!;
        public DbSet<EventEase3.Models.EventType> EventType { get; set; } = default!;
    }
}
