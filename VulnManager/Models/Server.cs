using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VulnManager.Data;
using System;

namespace VulnManager.Models
{
    public class Server
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        [Display(Name = "IP")]
        public string Ip { get; set; }
        [Display(Name = "Last Shodan update")]
        public DateTime LastShodanUpdate { get; set; }
        private readonly ApplicationDbContext _context;
        public ICollection<Port> Ports { get; } = new List<Port>();
        public ICollection<Vulnerability> Vulnerabilities { get; } = new List<Vulnerability>();
        public Server(string ip, ApplicationDbContext context)
        {
            Ip = ip;
            _context = context;
        }

        public async Task ChangeStateAsync(ShodanInfo shodanInfo, string serverId)
        {
            var server = _context.Servers.Where(s => s.Id == serverId).FirstOrDefault();
            await server.SetPortsAsync(shodanInfo.ports, serverId);
            await server.CreateCvesAsync(shodanInfo.vulns);
            await server.CreateVulnsAsync(shodanInfo.vulns, serverId);
            await _context.SaveChangesAsync();
        }

        public async Task CreateVulnsAsync(string[] vulns, string serverId)
        {
            if (vulns == null)
                return;
            foreach (var vuln in vulns)
            {
                var existingVuln = _context.Vulnerabilities.Where(v => v.CveName == vuln && v.ServerId == serverId);
                if(existingVuln == null)
                    continue;
                var vulnerability = new Vulnerability(serverId, vuln);
                await _context.AddAsync(vulnerability);
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
                if (existingCve == null)
                    continue;
                var cve = new Cve(cveInfo);
                _context.Add(cve);
            }
            await _context.SaveChangesAsync();
        }

        public async Task SetPortsAsync(int[] ports, string serverId)
        {
            if(ports == null || ports.Length == 0)
                return;
            foreach(var portInfo in ports)
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
