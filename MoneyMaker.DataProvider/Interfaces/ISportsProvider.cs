using MoneyMaker.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyMaker.DataProvider.Interfaces
{
    public interface ISportsProvider
    {
        Task<IEnumerable<SportDto>> GetSportsAsync();
    }
}
