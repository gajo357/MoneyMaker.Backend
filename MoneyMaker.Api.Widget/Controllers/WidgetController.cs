using Microsoft.AspNetCore.Mvc;
using MoneyMaker.DataProvider.Interfaces;
using MoneyMaker.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyMaker.Api.Widget.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WidgetController : ControllerBase
    {
        private IBookiesProvider BookiesProvider { get; }
        private ISportsProvider SportsProvider { get; }

        public WidgetController(IBookiesProvider bookiesProvider, ISportsProvider sportsProvider)
        {
            BookiesProvider = bookiesProvider;
            SportsProvider = sportsProvider;
        }

        [HttpGet]
        public async Task<IEnumerable<GameDto>> Get([FromQuery] string myBookie)
        {
            return await WidgetScraper.WidgetScraper.DownloadFromWidgetAsync(await SportsProvider.GetSportsAsync(), myBookie, await BookiesProvider.GetBookiesAsync());
        }
    }
}
