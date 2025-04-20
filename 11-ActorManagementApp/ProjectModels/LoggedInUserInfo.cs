using _11_ActorManagementApp.ProjectModels.Contracts;
using System.Security.Claims;

namespace _11_ActorManagementApp.ProjectModels
{
    public class LoggedInUserInfo : ILoggedInUserInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoggedInUserInfo(IHttpContextAccessor httpContext)
        {
            _httpContextAccessor = httpContext;
        }

        public string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public bool? IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
