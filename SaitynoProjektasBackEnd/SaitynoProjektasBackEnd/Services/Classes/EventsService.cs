using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.ResponseModels;
using SaitynoProjektasBackEnd.Services.Interfaces;

namespace SaitynoProjektasBackEnd.Services.Classes
{
    public class EventsService : IEventsService
    {
        private readonly ApplicationDbContext _context;

        public EventsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<EventResponseModel> GetEvents(string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var followedAuthIds = _context.Followings
                .Include(f => f.Follower)
                .Include(f => f.Followed)
                .Where(f => f.Follower == user)
                .Select(f => f.Followed.AuthId)
                .ToList();

            var events = _context.Events
                .Include(e => e.User)
                .Include(e => e.Song)
                    .ThenInclude(s => s.Genre)
                .Include(e => e.Song)
                    .ThenInclude(s => s.Likes)
                .Where(e => followedAuthIds.Contains(e.User.AuthId))
                .OrderByDescending(e => e.CreatedOn)
                .Select(Mappers.EventToEventResponseModel)
                .ToList();

            return events;
        }

        public IEnumerable<EventResponseModel> GetEventsLastWeek(string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var followedAuthIds = _context.Followings
                .Include(f => f.Follower)
                .Include(f => f.Followed)
                .Where(f => f.Follower == user)
                .Select(f => f.Followed.AuthId)
                .ToList();

            // TODO: Filtering is done on the backend, not on the database, because SQL Server does not support conversion from TimeSpan to DateTime.
            var events = _context.Events
                .Include(e => e.User)
                .Include(e => e.Song)
                    .ThenInclude(s => s.Genre)
                .Include(e => e.Song)
                    .ThenInclude(s => s.Likes)
                .Where(e => followedAuthIds.Contains(e.User.AuthId))
                .OrderByDescending(e => e.CreatedOn)
                .Select(Mappers.EventToEventResponseModel)
                .ToList();

            var filteredEvents = events
                .Where(e => e.CreatedOn + TimeSpan.FromDays(7) > DateTime.Now);

            return filteredEvents;
        }
    }
}
