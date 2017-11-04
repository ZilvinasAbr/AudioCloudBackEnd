using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;
using SaitynoProjektasBackEnd.Services.Interfaces;

namespace SaitynoProjektasBackEnd.Services.Classes
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _context;

        public UsersService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserResponseModel> GetUsers()
        {
            var users = _context.Users
                .ToList();

            return users.Select(Mappers.UserToUserResponseModel);
        }

        public UserResponseModel GetCurrentUser(ClaimsPrincipal userClaimsPrincipal)
        {
            var authId = GetUserAuthId(userClaimsPrincipal);
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            return Mappers.UserToUserResponseModel(user);
        }

        public UserResponseModel GetUserByName(string name)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == name);

            return user == null ? null : Mappers.UserToUserResponseModel(user);
        }

        public void EditUser(string authId, EditUserRequestModel userRequestModel)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
            {
                throw new Exception("User is not found");
            }

            if (!string.IsNullOrEmpty(userRequestModel.Name))
            {
                var isUserNameUsed = _context.Users.Any(u => u.UserName == userRequestModel.Name);

                if (isUserNameUsed)
                    throw new Exception("UserName is already used");

                user.UserName = userRequestModel.Name;
            }

            if (!string.IsNullOrEmpty(userRequestModel.Location))
            {
                user.Location = userRequestModel.Location;
            }

            if (!string.IsNullOrEmpty(userRequestModel.Description))
            {
                user.Description = userRequestModel.Description;
            }

            if (!string.IsNullOrEmpty(userRequestModel.ProfilePictureUrl))
            {
                user.Description = userRequestModel.ProfilePictureUrl;
            }

            _context.SaveChanges();
        }

        public string GetUserAuthId(ClaimsPrincipal claimsPrincipal)
        {
            var type = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

            var claims = claimsPrincipal.Claims.Select(c =>
                new
                {
                    c.Type,
                    c.Value
                });

            var authId = claims.SingleOrDefault(c => c.Type == type)?.Value;

            return authId;
        }

        public User RegisterUser(string authId)
        {
            var userIsFound = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (userIsFound!= null)
                throw new Exception("User is already registered");

            // TODO: Generates random username. Might need to change logic of this.
            var user = new User
            {
                AuthId = authId,
                UserName = $"user-{Guid.NewGuid()}"
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            
            return user;
        }

        public IEnumerable<UserResponseModel> GetUserFollowings(string userName)
        {
             var user = _context.Users
                .Include(u => u.Following)
                .SingleOrDefault(u => u.UserName == userName);

            if (user == null)
                throw new Exception("User is not found");

            var followings = _context.Followings
                .Include(f => f.Followed)
                .Where(f => f.FollowerId == user.Id)
                .Select(f => f.Followed)
                .Select(Mappers.UserToUserResponseModel)
                .ToList();

            return followings;
        }
    }
}
