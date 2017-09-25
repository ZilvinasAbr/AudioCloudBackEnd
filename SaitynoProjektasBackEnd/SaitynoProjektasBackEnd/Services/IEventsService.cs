using System.Collections.Generic;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface IEventsService
    {
        string[] GetEvents(string userName, out IEnumerable<EventResponseModel> events);
        string[] GetEventsLastWeek(string userName, out IEnumerable<EventResponseModel> events);
    }
}
