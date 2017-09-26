using System.Collections.Generic;
using System.Linq;
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

        public string[] EditUser(string name, EditUserRequestModel userRequestModel)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == name);

            if (user == null)
            {
                return new[] {"User is not found"};
            }

            if (!string.IsNullOrEmpty(userRequestModel.Name))
            {
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

        public string[] RegisterUser(string authId)
        {
            var userIsFound = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (userIsFound!= null)
                return new[] {"User is already registered"};

            var user = new User
            {
                AuthId = authId
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return null;
        }
    }
}
