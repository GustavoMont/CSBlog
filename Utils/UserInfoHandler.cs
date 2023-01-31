using System.Security.Claims;

namespace CSBlog.Utils;

public class UserInfoHandler
{
    private readonly IHttpContextAccessor _httpContext;

    public UserInfoHandler(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    protected int GetUserId()
    {
        var user = _httpContext.HttpContext.User;
        var id = Convert.ToInt32(user.FindFirst("id")?.Value);
        return id;
    }

    protected string GetUserRole()
    {
        var user = _httpContext.HttpContext.User;
        var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        return role;
    }
}
