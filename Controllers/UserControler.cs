using CSBlog.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Controllers;

[ApiController]
[Route("api/users")]
public class UserControler : ControllerBase
{
    private readonly UserService _service;

    public UserControler([FromServices] UserService userService)
    {
        _service = userService;
    }
}
