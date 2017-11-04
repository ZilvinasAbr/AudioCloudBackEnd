using System.Collections.Generic;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services.Interfaces
{
    public interface IEventsService
    {
        IEnumerable<EventResponseModel> GetEvents(string authId);
        IEnumerable<EventResponseModel> GetEventsLastWeek(string authId);
    }
}
