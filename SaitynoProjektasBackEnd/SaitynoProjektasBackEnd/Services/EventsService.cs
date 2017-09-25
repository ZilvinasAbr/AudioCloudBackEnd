using System;
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
                .ToList()
                .Select(Mappers.EventToEventResponseModel);

            return null;
        }

        public string[] GetEventsLastWeek(string userName, out IEnumerable<EventResponseModel> eventsResult)
        {
            eventsResult = null;
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName);

            if (user == null)
                return new[] { "User is not found" };

            var followedUserNames = _context.Followings
                .Include(f => f.Follower)
                .Include(f => f.Followed)
                .Where(f => f.Follower == user)
                .Select(f => f.Followed.UserName)
                .ToList();
            
            // TODO: Where, then ToList, and then Where again is used because somehow if I use two Where's one after another ant then ToList OR
            // TODO: if I put those Where's to one Where, sql exception happens. Should be investigated what's wrong with this.
            eventsResult = _context.Events
                .Include(e => e.User)
                .Include(e => e.Song)
                .Where(e => followedUserNames.Contains(e.User.UserName))
                .ToList()
                .Where(e => e.CreatedOn + TimeSpan.FromDays(7) > DateTime.Now)
                .Select(Mappers.EventToEventResponseModel);

            return null;
        }
    }
}
