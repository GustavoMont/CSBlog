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

    [HttpGet("post/{postId:int}")]
    public async Task<ActionResult<List<CommentRes>>> GetPostCommentsAsync([FromRoute] int postId)
    {
        try
        {
            var comments = await _service.GetPostCommentsAsync(postId);
            return Ok(comments);
        }
        catch (Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpPost("post/{postId:int}")]
    [Authorize]
    public async Task<ActionResult<CommentRes>> CreateAsync(
        [FromRoute] int id,
        [FromBody] CreateCommentReq body
    )
    {
        try
        {
            var newComment = await _service.CreateAsync(id, body);
            return CreatedAtAction(nameof(GetOneAsync), new { id = newComment.Id }, newComment);
        }
        catch (System.Exception err)
        {
            return BadRequest(new { message = err.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
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
