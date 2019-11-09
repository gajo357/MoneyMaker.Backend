using Microsoft.Extensions.Configuration;
using MoneyMaker.DataProvider.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MoneyMaker.DataProvider
{
    public class BookiesProvider : IBookiesProvider
    {
        private IEnumerable<string> Bookies { get; }

        public BookiesProvider(IConfiguration configuration)
        {
            //Bookies = configuration.GetValue<string[]>("Bookies");
            Bookies = JsonConvert.DeserializeObject<string[]>(File.ReadAllText("Bookies.json"));
        }

        public Task<IEnumerable<string>> GetBookiesAsync() => Task.FromResult(Bookies);
    }
}
