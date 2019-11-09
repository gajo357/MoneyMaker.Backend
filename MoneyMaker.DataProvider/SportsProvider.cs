using Microsoft.Extensions.Configuration;
using MoneyMaker.DataProvider.Interfaces;
using MoneyMaker.Dto;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MoneyMaker.DataProvider
{
    public class SportsProvider : ISportsProvider
    {
        private IEnumerable<SportDto> Sports { get; }

        public SportsProvider(IConfiguration configuration)
        {
            Sports = JsonConvert.DeserializeObject<SportDto[]>(File.ReadAllText("Sports.json"));
            //Sports = configuration.GetValue<SportDto[]>("Sports", null);
        }

        public Task<IEnumerable<SportDto>> GetSportsAsync() => Task.FromResult(Sports);
    }
}
