using System.ComponentModel.DataAnnotations;
using VulnManager.Data;

namespace VulnManager.Models
{
    public class Port
    {
        [Key]
        public int Id { get; set; }
        public Server Server { get; set; }
        public string ServerId { get; set; }
        private readonly ApplicationDbContext _context;


    }
}
