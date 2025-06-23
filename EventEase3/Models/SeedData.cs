using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EventEase3.Data;
using System;
using System.Linq;

namespace EventEase3.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EventEase3Context(
                serviceProvider.GetRequiredService<
                    DbContextOptions<EventEase3Context>>()))
            {
                // Look for any EventTypes.
                if (context.EventType.Any())
                {
                    return;   // DB has been seeded
                }

                context.EventType.AddRange(
                    new EventType { EventTypeName = "Conference" },
                    new EventType { EventTypeName = "Wedding" },
                    new EventType { EventTypeName = "Birthday" },
                    new EventType { EventTypeName = "Convention" },
                    new EventType { EventTypeName = "Concert" }
                );

                context.SaveChanges();
            }
        }
    }
}