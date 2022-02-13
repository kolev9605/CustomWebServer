namespace CustomWebServer.Console.Services
{
    public interface IUserService
    {
        bool IsLoginCorrect(string username, string password);
    }
}
