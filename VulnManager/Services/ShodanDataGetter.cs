using VulnManager.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VulnManager.Models;
using VulnManager.Services;
using System.Text.Json;
using System.Net;

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
            if(shodanResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Error: code: " + shodanResponse.StatusCode.ToString());
            }
            var shodanResponseString = await shodanResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ShodanInfo>(shodanResponseString);
        }

        public async Task SaveResults(ShodanInfo shodanInfo, string ip)
        {
            var server = _context.Servers.Where(s => s.Ip == ip).FirstOrDefault();
            await server.ChangeStateAsync(shodanInfo, server.Id);
        }

        public async Task ScanIpsAsync()
        {
            var serversIps = _context.Servers.Select(s => s.Ip);
            foreach (string ip in serversIps)
            {
                var url = new Uri("https://api.shodan.io/shodan/host/" + ip + "?key=" + apiKey);
                var shodanInfo = await ScanIpAsync(ip, url);
                await SaveResults(shodanInfo, ip);
            }
        }
    }
}
