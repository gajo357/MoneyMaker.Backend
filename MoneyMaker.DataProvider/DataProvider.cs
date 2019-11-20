using Microsoft.Extensions.Configuration;
using MoneyMaker.DataProvider.Interfaces;
using MoneyMaker.Dto;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MoneyMaker.DataProvider
{
    public class DataProvider : IDataProvider
    {
        private IEnumerable<SportDto> Sports { get; }
        private IEnumerable<LeagueGaussDto> Leagues { get; }
        private IEnumerable<BookieWidgetDto> Bookies { get; }


        public DataProvider(IConfiguration configuration)
        {
            Sports = JsonConvert.DeserializeObject<SportDto[]>(File.ReadAllText("Sports.json"));
            Leagues = JsonConvert.DeserializeObject<LeagueGaussDto[]>(File.ReadAllText("Leagues.json"));
            //Sports = configuration.GetValue<SportDto[]>("Sports", null);
            Bookies = JsonConvert.DeserializeObject<BookieWidgetDto[]>(File.ReadAllText("Bookies.json"));
        }

        public Task<IEnumerable<SportDto>> GetSportsAsync() => Task.FromResult(Sports);
        public Task<IEnumerable<LeagueGaussDto>> GetLeaguesAsync() => Task.FromResult(Leagues);
        public Task<IEnumerable<BookieWidgetDto>> GetBookiesAsync() => Task.FromResult(Bookies);
    }
}
