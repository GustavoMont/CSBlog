using CSBlog.Dtos.Comments;
using CSBlog.Exceptions;
using CSBlog.Models;
using CSBlog.Repositories;
using CSBlog.Utils;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Services;

public class CommentService : UserInfoHandler
{
    private readonly CommentRepository _repository;

    public CommentService(
        [FromServices] CommentRepository repository,
        [FromServices] IHttpContextAccessor contextAccessor
    ) : base(contextAccessor)
    {
        _repository = repository;
    }

    public async Task<CommentRes> CreateAsync(int postId, CreateCommentReq body)
    {
        var newComment = body.Adapt<Comment>();
        newComment.PostId = postId;
        newComment.UserId = GetUserId();
        var createdComment = await _repository.CreateAsync(newComment);
        return createdComment.Adapt<CommentRes>();
    }

    public async Task<CommentRes> GetOneAsync(int id)
    {
        var comment = await _repository.GetOneAsync(id);
        if (comment is null)
        {
            throw new NotFoundException("Comentário não encontrado");
        }
        return comment.Adapt<CommentRes>();
    }
}
