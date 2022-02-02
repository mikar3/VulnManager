using VulnManager.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VulnManager.Models;
using VulnManager.Services;
using System.Text.Json;

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

        public async Task GetInfoFromShodan(List<string> ipsToScan)
        {
            
            foreach (string ip in ipsToScan)
            {
                var connectionString = "https://api.shodan.io/shodan/host/" + ip + "?key=" + apiKey;
                var result = _httpClient.SendAsync()
            }
            var shodanInfo = JsonSerializer.Deserialize<ShodanInfo>(result);
        }




    }
}
