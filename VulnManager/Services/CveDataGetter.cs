using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using VulnManager.Data;
using VulnManager.Models;

namespace VulnManager.Services
{
    public class CveDataGetter
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public CveDataGetter(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task ScanCvesAsync()
        {
            var cves = await _context.Cves.ToListAsync();
            if (cves is null)
                return;
            foreach (var cve in cves)
            {
                var url = new Uri("https://olbat.github.io/nvdcve/" + cve.Name + ".json");
                var cveInfo = await ScanCveAsync(cve.Name, url);
                await UpdateCvssAsync(cveInfo, cve);
            }
        }

        public async Task UpdateCvssAsync(CveInfo cveInfo, Cve cve)
        {
            if (cveInfo == null)
            {
                return;
            }
            cve.CVSS = cveInfo.impact.baseMetricV2.cvssV2.baseScore;
            _context.Update(cve);
            await _context.SaveChangesAsync();
        }

        public async Task<CveInfo> ScanCveAsync(string ip, Uri uri)
        {
            var cveResponse = await _httpClient.GetAsync(uri);
            if (cveResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Error: code: " + cveResponse.StatusCode.ToString());
            }
            var cveResponseString = await cveResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CveInfo>(cveResponseString);
        }
    }
}
