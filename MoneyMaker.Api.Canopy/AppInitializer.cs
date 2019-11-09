using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MoneyMaker.Api.Canopy
{
    public class AppInitializer : IAsyncInitializer
    {
        public string Username { get; }
        public string Password { get; }

        public AppInitializer(IConfiguration configuration)
        {
            Username = configuration["User"];
            Password = configuration["Password"];
        }

        public async Task InitializeAsync()
        {
            CanopyScraper.CanopyNavigation.initialize();

            await CanopyScraper.CanopyScraper.logIn(Username, Password);
        }
    }
}
