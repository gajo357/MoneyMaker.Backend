using Microsoft.AspNetCore.Mvc;
using MoneyMaker.DataProvider.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyMaker.Api.Widget.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivelinessController : ControllerBase
    {
        private IDataProvider DataProvider { get; }

        public LivelinessController(IDataProvider sportsProvider)
        {
            DataProvider = sportsProvider;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get() => await Workflows.WidgetsActivityProbeAsync(await DataProvider.GetBookiesAsync());
    }
}
