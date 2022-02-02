using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VulnManager.Models;

namespace VulnManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public object AspNetUsers { get; internal set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Vulnerability> Vulnerabilities { get; set; }
        public DbSet<Cve> Cves { get; set; }
    }
}