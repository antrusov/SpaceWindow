using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceWindow
{
    public class Notifier
    {
        string _baseAddress;
        string _query;
        private HttpClient _client;

        public Notifier(string baseAddress, string query)
        {
            _baseAddress = baseAddress;
            _query = query;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseAddress);
        }

        public async Task Update (double x, double y, double vx, double vy)
        {            
            var q = string.Format(System.Globalization.CultureInfo.InvariantCulture, _query, x, y, vx, vy);
            
            Console.Write(q);
            Console.Write(" -> ");
            var result = await _client.GetStringAsync(q);
            Console.WriteLine(result);
        }
    }
}