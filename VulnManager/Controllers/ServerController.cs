using Microsoft.AspNetCore.Mvc;
using VulnManager.Models;
using VulnManager.Data;
using VulnManager.Services;

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
            _context.Remove(server);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Scan()
        {
            var client = new HttpClient();
            var shodanDataGetter = new Services.ShodanDataGetter(_context, client);
            await shodanDataGetter.ScanIpsAsync(_context);
            return RedirectToAction("Index");
        }



    }
}
