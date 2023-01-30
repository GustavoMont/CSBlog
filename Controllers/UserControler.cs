using CSBlog.Dtos.Token;
using CSBlog.Dtos.User;
using CSBlog.Exceptions;
using CSBlog.Models;
using CSBlog.Services;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{id:int}")]
    [ActionName(nameof(GetOneAsync))]
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

    [HttpPost("team")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<UserResponse>> CreateTeamUserAsync(
        [FromBody] CreateTeamUser body
    )
    {
        try
        {
            var newUser = await _service.CreateTeamUserAsync(body);
            return CreatedAtAction(nameof(GetOneAsync), new { id = newUser.Id }, newUser);
        }
        catch (System.Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<AuthToken>> CreateAsync([FromBody] CreateUserRequest body)
    {
        try
        {
            var response = await _service.CreateAsync(body);
            return StatusCode(201, response);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPost("login")]
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

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateUserReq body)
    {
        try
        {
            await _service.UpdateAsync(id, body);
            return Ok(new { message = "Usuário atualizado" });
        }
        catch (ForbiddenException)
        {
            return Forbid();
        }
        catch (NotFoundException err)
        {
            return NotFound(new { message = err.Message });
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult> SendResetPasswordEmailAsync([FromBody] SendEmailResetReq body)
    {
        try
        {
            await _service.SendResetPasswordEmailAsync(body.Email);
            return Ok(new { message = "E-mail de recuperação enviado" });
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordReq body)
    {
        try
        {
            await _service.ResetPasswordAsync(body);
            return Ok(new { message = "Senha redefinda com sucesso" });
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }
}
