using VulnManager.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VulnManager.Models;
using VulnManager.Services;
using System.Text.Json;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace VulnManager.Services
{
    public class ShodanDataGetter
    {
        private readonly ApplicationDbContext _context;
        private readonly string apiKey = "qNz6gnKXkbWdt1txXYHoyy5FV77YhD2W";
        private readonly HttpClient _httpClient;

        public ShodanDataGetter(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<ShodanInfo> ScanIpAsync(string ip, Uri uri)
        {
            var shodanResponse = await _httpClient.GetAsync(uri);
            if (shodanResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Error: code: " + shodanResponse.StatusCode.ToString());
            }
            var shodanResponseString = await shodanResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ShodanInfo>(shodanResponseString);
        }

        public async Task ScanIpsAsync()
        {
            var servers = await _context.Servers.ToListAsync();
            if (servers is null)
                return;
            foreach (var server in servers)
            {
                var url = new Uri("https://api.shodan.io/shodan/host/" + server.Ip + "?key=" + apiKey);
                var shodanInfo = await ScanIpAsync(server.Ip, url);
                await ChangeStateAsync(shodanInfo, server);
            }
        }

        public async Task ChangeStateAsync(ShodanInfo shodanInfo, Server server)
        {
            await SetPortsAsync(shodanInfo.ports, server.Id);
            await CreateCvesAsync(shodanInfo.vulns);
            await CreateVulnsAsync(shodanInfo.vulns, server.Id);
        }

        public async Task CreateVulnsAsync(string[] vulns, string serverId)
        {
            if (vulns == null)
                return;
            foreach (var vuln in vulns)
            {
                var existingVuln = _context.Vulnerabilities.Where(v => v.CveName == vuln && v.ServerId == serverId).FirstOrDefault();
                if (existingVuln != null)
                    continue;
                var vulnerability = new Vulnerability(vuln, serverId);
                _context.Add(vulnerability);
            }
            await _context.SaveChangesAsync();
        }

        public async Task CreateCvesAsync(string[] cves)
        {
            if (cves == null)
                return;
            foreach (var cveInfo in cves)
            {
                var existingCve = _context.Cves.Where(c => c.Name == cveInfo).FirstOrDefault();
                if (existingCve != null)
                    continue;
                var cve = new Cve(cveInfo);
                _context.Add(cve);
            }
            await _context.SaveChangesAsync();
        }

        public async Task SetPortsAsync(int[] ports, string serverId)
        {
            if (ports == null || ports.Length == 0)
                return;
            foreach (var portInfo in ports)
            {
                var existingPort = _context.Ports.Where(p => p.PortNr == portInfo && p.ServerId == serverId).FirstOrDefault();
                if (existingPort == null)
                    continue;
                var port = new Port(portInfo, serverId);
                await _context.AddAsync(port);
            }
            await _context.SaveChangesAsync();
        }
    }
}
