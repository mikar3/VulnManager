using Microsoft.AspNetCore.Mvc;
using VulnManager.Models;
using VulnManager.Data;
using VulnManager.Services;
using Microsoft.EntityFrameworkCore;

namespace VulnManager.Controllers
{
    public class CVEController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CVEController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cves = _context.Cves.ToList();
            return View(cves);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCves()
        {
            var client = new HttpClient();
            var cveDataGetter = new CveDataGetter(_context, client);
            await cveDataGetter.ScanCvesAsync();
            return RedirectToAction("Index");
        }
    }
}