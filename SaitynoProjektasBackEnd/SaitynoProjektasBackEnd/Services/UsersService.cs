﻿using System.Collections.Generic;
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
                user.NormalizedUserName = userRequestModel.Name.Normalize();
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
    }
}
