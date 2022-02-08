using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VulnManager.Models;
using VulnManager.Data;

namespace VulnManager.Controllers
{
    public class ServerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ServerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var servers = _context.Servers.ToList();
            return View(servers);
        }

        public IActionResult Create()
		{
            return View();
		}

        [HttpPost]
        public async Task<IActionResult> Create(string ip)
		{
            var server = new Server() { Ip = ip };
            _context.Add(server);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
		}

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var server = _context.Servers.Where(s => s.Id == id).FirstOrDefault();
            if (server == null)
            {
                return NotFound();
            }
            return View(server);
        }





    }
}
