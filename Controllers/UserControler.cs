using CSBlog.Dtos.Token;
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

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> ListAsync()
    {
        try
        {
            var response = await _service.ListAsync();
            return Ok(response);
        }
        catch (HttpRequestException err)
        {
            return NotFound(new { message = err.Message });
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetOneAsync([FromRoute] int id)
    {
        try
        {
            var user = await _service.GetOneAsync(id);
            return Ok(user);
        }
        catch (HttpRequestException err)
        {
            return NotFound(new { message = err.Message });
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
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

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthToken>> LoginAsync([FromBody] LoginReq credential)
    {
        try
        {
            var response = await _service.LoginAsync(credential);
            return Ok(response);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }
}
