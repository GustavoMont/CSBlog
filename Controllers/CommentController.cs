using CSBlog.Dtos.Comments;
using CSBlog.Exceptions;
using CSBlog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentController : ControllerBase
{
    private readonly CommentService _service;

    public CommentController([FromServices] CommentService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    [ActionName(nameof(GetOneAsync))]
    public async Task<ActionResult<CommentRes>> GetOneAsync(int id)
    {
        try
        {
            var comment = await _service.GetOneAsync(id);
            return Ok(comment);
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

    [HttpPost("post/{id:int")]
    [Authorize]
    public async Task<ActionResult<CommentRes>> CreateAsync(
        [FromRoute] int postId,
        [FromBody] CreateCommentReq body
    )
    {
        try
        {
            var newComment = await _service.CreateAsync(postId, body);
            return CreatedAtAction(nameof(GetOneAsync), new { id = newComment.Id }, newComment);
        }
        catch (System.Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }
}
