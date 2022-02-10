using System.ComponentModel.DataAnnotations;
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


    }
}
