using System.Security.Claims;
using CSBlog.Dtos;
using CSBlog.Exceptions;

namespace CSBlog.Utils;

public class ServiceUtils
{
    private readonly IHttpContextAccessor _httpContext;

    public ServiceUtils(IHttpContextAccessor httpContext)
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

    protected void HandlePagination(int take)
    {
        if (take > 500)
        {
            throw new ForbiddenException();
        }
    }

    protected int GenerateSkip(int page, int take)
    {
        return (page - 1) * take;
    }
}
