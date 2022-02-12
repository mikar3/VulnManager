using Microsoft.AspNetCore.Mvc;
using VulnManager.Models;
using VulnManager.Data;
using VulnManager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace VulnManager.Controllers
{
    [Authorize]
    public class PortController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PortController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ports = _context.Ports.Include(s => s.Server).ToList();
            return View(ports);
        }
    }
}