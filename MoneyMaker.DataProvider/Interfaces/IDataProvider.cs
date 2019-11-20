using MoneyMaker.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyMaker.DataProvider.Interfaces
{
    public interface IDataProvider
    {
        Task<IEnumerable<SportDto>> GetSportsAsync();
        Task<IEnumerable<LeagueGaussDto>> GetLeaguesAsync();
        Task<IEnumerable<BookieWidgetDto>> GetBookiesAsync();
    }
}
