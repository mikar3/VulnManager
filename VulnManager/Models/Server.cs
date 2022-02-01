using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VulnManager.Data;

namespace VulnManager.Models
{
    public class Server
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string Ip { get; set; }
        private readonly ApplicationDbContext _context;
        public ICollection<Port> Ports { get; } = new List<Port>();
        public ICollection<Vulnerability> Vulnerabilities { get; } = new List<Vulnerability>();
        public Server(string ip, ApplicationDbContext context)
        {
            Ip = ip;
            _context = context;
        }

    }
}
