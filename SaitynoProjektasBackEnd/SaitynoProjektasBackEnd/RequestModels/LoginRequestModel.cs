namespace SaitynoProjektasBackEnd.RequestModels
{
    public class LoginRequestModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
