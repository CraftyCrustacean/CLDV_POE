using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase3.Data;
using EventEase3.Models;
using System.Configuration;
using Azure.Storage.Blobs;

namespace EventEase3.Controllers
{
    public class VenuesController : Controller
    {
        private readonly EventEase3Context _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public VenuesController(EventEase3Context context, IConfiguration config)
        {
            _context = context;
            _blobServiceClient = new BlobServiceClient(config 
                ["AzureBlobStorage:ConnectionString"]);
            _containerName = config["AzureBlobStorage:ContainerName"];
        }

        // GET: Venues
        public async Task<IActionResult> Index(string searchString, bool? availabilityFilter)
        {
            if (_context.Venue == null)
            {
                return Problem("Entity set is null.");
            }

            var venues = from v in _context.Venue
                         select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                venues = venues.Where(s => s.VenueName!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (availabilityFilter.HasValue)
            {
                venues = venues.Where(v => v.Availability == availabilityFilter.Value);
            }

            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentAvailabilityFilter"] = availabilityFilter;
            return View(await venues.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public async Task<IActionResult> Create(IFormFile imageFile, string name)
        {

            return View();
        }

        // POST: Venues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile imageFile, [Bind("VenueName,VenueLocal,VenueCap,Availability")] Venue venue)
        {
         
            if (imageFile == null) return BadRequest("No image uploaded.");

            var container = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blob = container.GetBlobClient(imageFile.FileName);

            using var stream = imageFile.OpenReadStream();
            await blob.UploadAsync(stream, overwrite: true);

            var existing = _context.Venue.Where(v => v.VenueName.StartsWith(venue.VenueName)).ToList();
            venue.VenueName = existing.Count > 0 ? $"{venue.VenueName}{existing.Count + 1}" : venue.VenueName;
            venue.VenueImgURL = blob.Uri.ToString();

            _context.Venue.Add(venue);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        // POST: Venues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile? imageFile, [Bind("VenueId,VenueName,VenueLocal,VenueCap,VenueImgURL, Availability")] Venue venue)
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var container = _blobServiceClient.GetBlobContainerClient(_containerName);

                    // Handle image replacement
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete the old image
                        if (!string.IsNullOrEmpty(venue.VenueImgURL))
                        {
                            var oldFileName = Path.GetFileName(new Uri(venue.VenueImgURL).LocalPath);
                            var oldBlob = container.GetBlobClient(oldFileName);
                            await oldBlob.DeleteIfExistsAsync();
                        }

                        // Upload the new image
                        var newBlob = container.GetBlobClient(imageFile.FileName);
                        using var stream = imageFile.OpenReadStream();
                        await newBlob.UploadAsync(stream, overwrite: true);

                        venue.VenueImgURL = newBlob.Uri.ToString();
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(venue);
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venue
                .Include(v => v.Bookings)
                .FirstOrDefaultAsync(v => v.VenueId == id);

            if (venue == null)
            {
                return NotFound();
            }

            if (venue.Bookings != null && venue.Bookings.Any())
            {
                TempData["Error"] = "Cannot delete this venue because it has active bookings.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            // Delete image from Blob Storage
            if (!string.IsNullOrEmpty(venue.VenueImgURL))
            {
                var container = _blobServiceClient.GetBlobContainerClient(_containerName);
                var fileName = Path.GetFileName(new Uri(venue.VenueImgURL).LocalPath);
                var blobClient = container.GetBlobClient(fileName);
                await blobClient.DeleteIfExistsAsync();
            }

            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueId == id);
        }
    }
}
