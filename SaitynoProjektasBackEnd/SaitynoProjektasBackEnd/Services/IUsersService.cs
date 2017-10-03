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
        void EditUser(string authId, EditUserRequestModel user);
        User RegisterUser(string authId);
        string GetUserAuthId(ClaimsPrincipal claimsPrincipal);
    }
}
