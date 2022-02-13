namespace CustomWebServer.Console.Services
{
    public class UserService : IUserService
    {
        private const string Username = "user";

        private const string Password = "user123";

        public bool IsLoginCorrect(string username, string password)
        {
            return username == Username && password == Password;
        }
    }
}
