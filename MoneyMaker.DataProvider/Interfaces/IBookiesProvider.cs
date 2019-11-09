using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyMaker.DataProvider.Interfaces
{
    public interface IBookiesProvider
    {
        Task<IEnumerable<string>> GetBookiesAsync();
    }
}
