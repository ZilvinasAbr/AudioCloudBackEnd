using System.Collections;
using System.Collections.Generic;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface IUsersService
    {
        IEnumerable<UserResponseModel> GetUsers();
        UserResponseModel GetUserByName(string name);
        string[] EditUser(string name, EditUserRequestModel user);
    }
}
