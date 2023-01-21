using CSBlog.Dtos.Posts;
using CSBlog.Exceptions;
using CSBlog.Models;
using CSBlog.Repositories;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Services;

public class PostService
{
    private readonly PostRepository _repository;
    private readonly IHttpContextAccessor _httpContext;

    public PostService(
        [FromServices] PostRepository repository,
        [FromServices] IHttpContextAccessor httpContext
    )
    {
        _repository = repository;
        _httpContext = httpContext;
    }

    private int GetUserId()
    {
        var HttpContext = _httpContext.HttpContext;
        var id = Convert.ToInt32(HttpContext.User.FindFirst("id").Value);
        return id;
    }

    public async Task<PostRes> CreateAsync(CreatePost body)
    {
        var hasStatus = body.Status is not null;
        if (hasStatus && !Enum.IsDefined(typeof(PostStatus), body.Status))
        {
            throw new BadHttpRequestException("Status inválido");
        }
        var newPost = body.Adapt<Post>();
        newPost.AuthorId = GetUserId();
        newPost.Create();
        var post = await _repository.CreateAsync(newPost);
        return post.Adapt<PostRes>();
    }

    public async Task<List<PostRes>> ListAsync()
    {
        var posts = await _repository.ListAsync();
        if (posts.Count <= 0)
        {
            throw new NotFoundException("Nenhum post foi feito");
        }
        return posts.Adapt<List<PostRes>>();
    }

    public async Task<PostRes> GetOneAsync(int id)
    {
        var post = await _repository.GetOneAsync(id);
        if (post is null)
        {
            throw new NotFoundException("Post não encontrado");
        }
        return post.Adapt<PostRes>();
    }
}
