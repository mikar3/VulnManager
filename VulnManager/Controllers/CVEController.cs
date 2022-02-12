using Microsoft.AspNetCore.Mvc;
using VulnManager.Models;
using VulnManager.Data;
using VulnManager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace VulnManager.Controllers
{
    [Authorize]
    public class CVEController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CVEController> _logger;
        public CVEController(ApplicationDbContext context, ILogger<CVEController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var cves = _context.Cves.ToList();
            return View(cves);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCves()
        {
            _logger.LogInformation($"{User.Identity.Name} updates CVE at {DateTime.Now}");
            var client = new HttpClient();
            var cveDataGetter = new CveDataGetter(_context, client);
            await cveDataGetter.ScanCvesAsync();
            return RedirectToAction("Index");
        }
    }
}