using Microsoft.AspNetCore.Mvc;
using VulnManager.Models;
using VulnManager.Data;
using VulnManager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace VulnManager.Controllers
{
    [Authorize]
    public class ServerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CVEController> _logger;
        public ServerController(ApplicationDbContext context, ILogger<CVEController> logger)
        {
            _context = context;
            _logger = logger;
    }

        public IActionResult Index()
        {
            var servers = _context.Servers.Include(v => v.Vulnerabilities).Include(p => p.Ports).ToList();
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
            _logger.LogInformation($"{User.Identity.Name} creates server {server.Ip} at {DateTime.Now}");
            return RedirectToAction("Index");
		}

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var server = _context.Servers.Include(v=>v.Vulnerabilities).Include(p=>p.Ports).Where(s => s.Id == id).FirstOrDefault();
            if (server == null)
            {
                return NotFound();
            }
            return View(server);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var server = _context.Servers.Where(s => s.Id == id).FirstOrDefault();
            return View(server);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if(id is null)
            {
                return NotFound();
            }
            var server = _context.Servers.Where(s => s.Id == id).FirstOrDefault();
            _logger.LogInformation($"{User.Identity.Name} deletes server {server.Ip} at {DateTime.Now}");
            _context.Remove(server);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Scan()
        {
            var client = new HttpClient();
            var shodanDataGetter = new Services.ShodanDataGetter(_context, client);
            _logger.LogInformation($"{User.Identity.Name} scans server at {DateTime.Now}");
            await shodanDataGetter.ScanIpsAsync();
            return RedirectToAction("Index");
        }



    }
}
