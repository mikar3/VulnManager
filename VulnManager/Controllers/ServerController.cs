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
            return View();
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
