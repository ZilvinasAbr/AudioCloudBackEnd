using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
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

        public UserResponseModel GetUserByName(string name)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == name);

            return user == null ? null : Mappers.UserToUserResponseModel(user);
        }

        public string[] EditUser(string authId, EditUserRequestModel userRequestModel)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
            {
                return new[] {"User is not found"};
            }

            if (!string.IsNullOrEmpty(userRequestModel.Name))
            {
                var isUserNameUsed = _context.Users.Any(u => u.UserName == userRequestModel.Name);

                if (isUserNameUsed)
                    return new[] {"UserName is already used"};

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

            return null;
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

        public string[] RegisterUser(string authId)
        {
            var userIsFound = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (userIsFound!= null)
                return new[] {"User is already registered"};

            // TODO: Generates random username. Might need to change logic of this.
            var user = new User
            {
                AuthId = authId,
                UserName = $"user-{Guid.NewGuid()}"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return null;
        }
    }
}
