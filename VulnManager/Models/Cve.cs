using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VulnManager.Data;

namespace VulnManager.Models
{
    public class Cve
    {
        [Key]
        public string Name { get; set; }
        public double CVSS { get; set; }
        private readonly ApplicationDbContext _context;
        public ICollection<Vulnerability> Vulnerabilities { get; } = new List<Vulnerability>();

        public Cve(string name)
        {
            Name = name;
        }




    }
}
