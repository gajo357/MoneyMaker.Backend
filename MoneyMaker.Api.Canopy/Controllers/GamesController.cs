using Microsoft.AspNetCore.Mvc;
using MoneyMaker.DataProvider.Interfaces;
using MoneyMaker.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyMaker.Api.Canopy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private IBookiesProvider BookiesProvider { get; }
        private ISportsProvider SportsProvider { get; }

        public GamesController(IBookiesProvider bookiesProvider, ISportsProvider sportsProvider)
        {
            BookiesProvider = bookiesProvider;
            SportsProvider = sportsProvider;
        }

        [HttpGet]
        public async Task<IEnumerable<GameDto>> GetGames([FromQuery] DateTime? date) 
            => await CanopyScraper.CanopyScraper.downloadGameInfos(await SportsProvider.GetSportsAsync(), date ?? DateTime.Now);
        
        [HttpPost]
        public async Task<GameDto> GameFromLink([FromBody] GameLink linkDto)
            => await CanopyScraper.CanopyScraper.readGameFromLink(linkDto.Bookie, await BookiesProvider.GetBookiesAsync(), linkDto.Link);
    }

    public class GameLink
    {
        public string Bookie { get; set; }
        public string Link { get; set; }
    }
}
