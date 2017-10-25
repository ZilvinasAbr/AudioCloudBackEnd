using System.Collections.Generic;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface IGenresService
    {
        IEnumerable<string> GetGenres();
    }
}
