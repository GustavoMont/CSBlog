using CSBlog.Dtos;
using CSBlog.Dtos.Posts;
using CSBlog.Exceptions;
using CSBlog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController : ControllerBase
{
    private readonly PostService _service;

    public PostController([FromServices] PostService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ListResponse<PostRes>>> ListAsync(
        [FromQuery] int page = 1,
        [FromQuery] int take = 25
    )
    {
        try
        {
            var posts = await _service.ListPublishedAsync(page, take);
            return Ok(posts);
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

    [HttpGet("all")]
    [Authorize(Roles = "ADMIN, WRITER")]
    public async Task<ActionResult<ListResponse<PostRes>>> ListAllAsync(
        [FromQuery] int page = 1,
        [FromQuery] int take = 25
    )
    {
        try
        {
            var posts = await _service.ListAsync(page, take);
            return Ok(posts);
        }
        catch (ForbiddenException)
        {
            return Forbid();
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpGet("{id:int}")]
    [ActionName(nameof(GetOneAsync))]
    public async Task<ActionResult<PostRes>> GetOneAsync([FromRoute] int id)
    {
        try
        {
            var post = await _service.GetOneAsync(id);
            return post;
        }
        catch (NotFoundException err)
        {
            return NotFound(new { message = err.Message });
        }
        catch (ForbiddenException)
        {
            return Forbid();
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN, WRITER")]
    public async Task<ActionResult<PostRes>> CreateAsync([FromBody] CreatePost body)
    {
        try
        {
            var newPost = await _service.CreateAsync(body);
            return CreatedAtAction(nameof(GetOneAsync), new { id = newPost.Id }, newPost);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "ADMIN, WRITER")]
    public async Task<ActionResult<PostRes>> UpdateAsync(
        [FromRoute] int id,
        [FromBody] CreatePost changes
    )
    {
        try
        {
            var postUpdated = await _service.UpdateAsync(id, changes);
            return Ok(postUpdated);
        }
        catch (NotFoundException err)
        {
            return NotFound(new { message = err.Message });
        }
        catch (ForbiddenException)
        {
            return Forbid();
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN, WRITER")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (NotFoundException err)
        {
            return NotFound(new { message = err.Message });
        }
        catch (ForbiddenException)
        {
            return Forbid();
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }
}
