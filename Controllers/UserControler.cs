using CSBlog.Dtos.User;
using CSBlog.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController([FromServices] UserService userService)
    {
        _service = userService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] CreateUserRequest body)
    {
        try
        {
            await _service.CreateAsync(body);
            return StatusCode(201, new { message = "Usuário criado com sucesso" });
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }
}
