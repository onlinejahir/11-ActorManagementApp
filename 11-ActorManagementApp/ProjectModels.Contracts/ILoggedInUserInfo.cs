namespace _11_ActorManagementApp.ProjectModels.Contracts
{
    public interface ILoggedInUserInfo
    {
        string? GetUserId();
        bool? IsAuthenticated();
    }
}
