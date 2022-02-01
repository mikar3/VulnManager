using Shodan.Client;
using Shodan.Models;
using Shodan.Search;
using VulnManager.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VulnManager.Models;
using VulnManager.Services;


namespace VulnManager.Services
{
    public class ShodanDataGetter
    {
        private readonly ApplicationDbContext _context;
        private readonly string apiKey = "qNz6gnKXkbWdt1txXYHoyy5FV77YhD2W";

        public ShodanDataGetter(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task ScanIpAsync(string ip, ShodanClient client)
        {
            ShodanSearchQuery query = new ShodanSearchQuery { IP = ip };
            SearchResult result = await client.Search(query);
           // await SaveResultAsync(result);
        }

        //public Task SaveResultAsync(SearchResult result)
        //{
        //    //var server = _context.Model.
        //}


        public async Task GetShodanDataAsync(List<string> ipsToScan)
        {
            ShodanClient client = new ClientFactory(apiKey).GetFullClient();
            foreach (string ip in ipsToScan)
            {
                await ScanIpAsync(ip, client);
            }
        }
    }
}
