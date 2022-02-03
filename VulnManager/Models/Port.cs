using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VulnManager.Data;

namespace VulnManager.Models
{
    public class Port
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int PortNr { get; set; }
        public Server Server { get; set; }
        public string ServerId { get; set; }
        private readonly ApplicationDbContext _context;


    }
}
