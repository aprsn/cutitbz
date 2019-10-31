using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using CutitBz.Data;
using CutitBz.Hubs;
using CutitBz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CutitBz.Controllers
{
    public class LinksController : Controller
    {

        public readonly DataContext _context;
        private readonly IHubContext<StatusHub> _hubContext;
        public LinksController(DataContext context, IHubContext<StatusHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public async Task<IActionResult> ViewLink(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var links = await _context.Links
                .FirstOrDefaultAsync(m => m.id == id && m.displayed == 0);
            if (links == null)
            {
                return NotFound();
            }

            links.displayed = 1;
            _context.Update(links);
            await _context.SaveChangesAsync();
            return View(links);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetStatus([Bind("shortLink", "pin")] Link link)
        {


            if(link.shortLink == null || link.pin == null) {
                return BadRequest("Please do not leave empty!");
            }

            if (link.shortLink.StartsWith("http://cutit.bz/"))
            {
                link.shortLink = link.shortLink.Substring("http://cutit.bz/".Length);
            }
            else if (link.shortLink.StartsWith("https://cutit.bz/"))
            {
                link.shortLink = link.shortLink.Substring("https://cutit.bz/".Length);
            }
            else if (link.shortLink.StartsWith("cutit.bz/"))
            {
                link.shortLink = link.shortLink.Substring("cutit.bz/".Length);
            }
            var links = await _context.Links
                .FirstOrDefaultAsync(m => m.shortLink == link.shortLink && m.pin == link.pin);

            if (links == null)
            {
                return NotFound();
            }

            return View(links);
        }

        public IActionResult Status()
        {
            return View();
        }



    }
}