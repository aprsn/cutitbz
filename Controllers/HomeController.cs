using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CutitBz.Models;
using System.Text;
using CutitBz.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Caching.Distributed;
using CutitBz.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CutitBz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache _cache;
        public readonly DataContext _context;
        private readonly IHubContext<StatusHub> _hubContext;

        public HomeController(ILogger<HomeController> logger, DataContext context, IDistributedCache cache, IHubContext<StatusHub> hubContext)
        {
            _logger = logger;
            _context = context;
            _cache = cache;
            _hubContext = hubContext;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("fullLink")] Link link)
        {
            if (link.fullLink == null || link.fullLink == "") { return BadRequest("Please enter a valid URL!"); }
            if (ModelState.IsValid)
            {
                link.shortLink = CreatePassword(4);
                link.pin = CreatePassword(5);
                link.created = DateTime.Now;
                _context.Add(link);
                await _context.SaveChangesAsync();

                byte[] value = Encoding.UTF8.GetBytes(link.fullLink);
                _cache.Set(link.shortLink, value);

                return Redirect("/Links/ViewLink/" + link.id);
            }
            return View(link);
        }


        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "";
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Go(string id)
        {

            if (id == null)
            {
                return NotFound("Link does not valid!");
            }

            var getCache = _cache.Get(id);
            var cachedLink = Encoding.UTF8.GetString(getCache);

            if (cachedLink != null)
            {
                Redirect(cachedLink);
            }

            var links = await _context.Links
                      .FirstOrDefaultAsync(l => l.shortLink == id);

            if (links == null)
            {
                return NotFound("There is no any result!");
            }


            var views = await _context.Views
                                     .FirstOrDefaultAsync(w => w.ip == GetLocalIPAddress() && w.link == id);
            if (views == null)
            {
                try
                {
                    links.views = links.views + 1;
                    _context.Update(links);
                    await _context.SaveChangesAsync();
                    var viewsModel = new View();
                    viewsModel.ip = GetLocalIPAddress();
                    viewsModel.link = id;
                    viewsModel.date = DateTime.Now;
                    _context.Add(viewsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound("'View' error!");
                }
            }
            try
            {
                links.hits = links.hits + 1;

                await _hubContext.Clients.Group(id).SendAsync("ReceiveStatus", links.hits, links.views);
                _context.Update(links);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("'Hits' error!");
            }

            return Redirect(links.fullLink);
        }


    }
}
