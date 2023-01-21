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
    public async Task<ActionResult<List<PostRes>>> ListAsync()
    {
        try
        {
            var posts = await _service.ListAsync();
            return Ok(posts);
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
}
