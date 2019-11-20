using Microsoft.AspNetCore.Mvc;
using MoneyMaker.DataProvider.Interfaces;
using MoneyMaker.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyMaker.Api.Canopy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private IDataProvider DataProvider { get; }

        public GamesController(IDataProvider sportsProvider)
        {
            DataProvider = sportsProvider;
        }

        [HttpGet]
        public async Task<IEnumerable<GameDto>> GetGames([FromQuery] DateTime? date) 
            => await CanopyScraper.CanopyScraper.downloadGameInfos(await DataProvider.GetSportsAsync(), date ?? DateTime.Now);
        
        [HttpPost]
        public async Task<GameDto> GameFromLink([FromBody] GameLink linkDto)
            => await CanopyScraper.CanopyScraper.readGameFromLink((await DataProvider.GetBookiesAsync()).Select(s => s.Name), linkDto.Link);
    }

    public class GameLink
    {
        public string Bookie { get; set; }
        public string Link { get; set; }
    }
}
