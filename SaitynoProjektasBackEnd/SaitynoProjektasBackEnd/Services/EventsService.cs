using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public class EventsService : IEventsService
    {
        private readonly ApplicationDbContext _context;

        public EventsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public string[] GetEvents(string userName, out IEnumerable<EventResponseModel> eventsResult)
        {
            eventsResult = null;
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName);

            if (user == null)
                return new[] {"User is not found"};

            var followedUserNames = _context.Followings
                .Include(f => f.Follower)
                .Include(f => f.Followed)
                .Where(f => f.Follower == user)
                .Select(f => f.Followed.UserName)
                .ToList();

            eventsResult = _context.Events
                .Include(e => e.User)
                .Include(e => e.Song)
                .Where(e => followedUserNames.Contains(e.User.UserName))
                .AsEnumerable()
                .Select(Mappers.EventToEventResponseModel);

            return null;
        }
    }
}
