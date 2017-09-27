using System.Collections.Generic;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface IEventsService
    {
        string[] GetEvents(string authId, out IEnumerable<EventResponseModel> events);
        string[] GetEventsLastWeek(string authId, out IEnumerable<EventResponseModel> events);
    }
}
