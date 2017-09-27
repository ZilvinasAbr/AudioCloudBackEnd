using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface IUsersService
    {
        IEnumerable<UserResponseModel> GetUsers();
        UserResponseModel GetUserByName(string name);
        string[] EditUser(string authId, EditUserRequestModel user);
        string[] RegisterUser(string authId);
        string GetUserAuthId(ClaimsPrincipal claimsPrincipal);
    }
}
