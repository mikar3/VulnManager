﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VulnManager.Data;
using System;
using Microsoft.EntityFrameworkCore;

namespace VulnManager.Models
{
    [Index(nameof(Ip), IsUnique = true)]
    public class Server
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        [Display(Name = "IP")]
        public string Ip { get; set; }
        [Display(Name = "Last Shodan update")]
        public DateTime? LastShodanUpdate { get; set; }
        private readonly ApplicationDbContext _context;
        public ICollection<Port> Ports { get; } = new List<Port>();
        public ICollection<Vulnerability> Vulnerabilities { get; } = new List<Vulnerability>();
        public Server(string ip, ApplicationDbContext context)
        {
            Ip = ip;
            _context = context;
        }

		public Server()
		{ }

		public async Task ChangeStateAsync(ShodanInfo shodanInfo, Server server, ApplicationDbContext context)
        {
            await server.SetPortsAsync(shodanInfo.ports, server.Id, context);
            await server.CreateCvesAsync(shodanInfo.vulns, context);
            await server.CreateVulnsAsync(shodanInfo.vulns, server.Id, context);
            await _context.SaveChangesAsync();
        }

        public async Task CreateVulnsAsync(string[] vulns, string serverId, ApplicationDbContext context)
        {
            if (vulns == null)
                return;
            foreach (var vuln in vulns)
            {
                var existingVuln = context.Vulnerabilities.Where(v => v.CveName == vuln && v.ServerId == serverId);
                if(existingVuln == null)
                    continue;
                var vulnerability = new Vulnerability(serverId, vuln);
                await _context.AddAsync(vulnerability);
            }
            await _context.SaveChangesAsync();
        }

        public async Task CreateCvesAsync(string[] cves, ApplicationDbContext context)
        {
            if (cves == null)
                return;
            foreach (var cveInfo in cves)
            {
                var existingCve = context.Cves.Where(c => c.Name == cveInfo).FirstOrDefault();
                if (existingCve == null)
                    continue;
                var cve = new Cve(cveInfo);
                _context.Add(cve);
            }
            await _context.SaveChangesAsync();
        }

        public async Task SetPortsAsync(int[] ports, string serverId, ApplicationDbContext context)
        {
            if(ports == null || ports.Length == 0)
                return;
            foreach(var portInfo in ports)
            {
                var existingPort = context.Ports.Where(p => p.PortNr == portInfo && p.ServerId == serverId).FirstOrDefault();
                if (existingPort == null)
                    continue;
                var port = new Port(portInfo, serverId);
                await _context.AddAsync(port);
            }
            await _context.SaveChangesAsync();
        }

    }
}
