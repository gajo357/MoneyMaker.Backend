﻿using Microsoft.AspNetCore.Mvc;
using MoneyMaker.DataProvider.Interfaces;
using MoneyMaker.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyMaker.Api.Widget.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WidgetController : ControllerBase
    {
        private IDataProvider DataProvider { get; }

        public WidgetController(IDataProvider sportsProvider)
        {
            DataProvider = sportsProvider;
        }

        [HttpGet]
        public async Task<IEnumerable<GameWithBetDto>> Get([FromQuery] string myBookie) 
            => await Workflows.DownloadFromWidgetAsync(await DataProvider.GetLeaguesAsync(), myBookie, await DataProvider.GetBookiesAsync());
    }
}
