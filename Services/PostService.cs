using System.Security.Claims;
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
        var user = _httpContext.HttpContext.User;
        var id = Convert.ToInt32(user.FindFirst("id")?.Value);
        return id;
    }

    private string GetUserRole()
    {
        var user = _httpContext.HttpContext.User;
        var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        return role;
    }

    private bool IsAdmin()
    {
        var role = GetUserRole();
        return role == UserType.ADMIN.ToString();
    }

    private bool IsReader()
    {
        var role = GetUserRole();
        return role is null || role == UserType.READER.ToString();
    }

    private async Task<Post> GetByIdAsync(int id, bool tracking = true)
    {
        var post = await _repository.GetOneAsync(id, tracking);
        if (post is null)
        {
            throw new NotFoundException("Post não encontrado");
        }
        else if (post.Status == PostStatus.DRAFT && !IsAdmin())
        {
            if (IsReader() || post.AuthorId != GetUserId())
            {
                throw new ForbiddenException();
            }
        }
        return post;
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
        var posts = await _repository.ListAsync(IsAdmin() ? null : GetUserId());
        return posts.Adapt<List<PostRes>>();
    }

    public async Task<List<PostRes>> ListPublishedAsync()
    {
        var posts = await _repository.ListPublishedAsync();
        if (posts.Count <= 0)
        {
            throw new NotFoundException("Nenhum post foi feito");
        }
        return posts.Adapt<List<PostRes>>();
    }

    public async Task<PostRes> GetOneAsync(int id)
    {
        var post = await GetByIdAsync(id, false);

        return post.Adapt<PostRes>();
    }

    public async Task<PostRes> UpdateAsync(int id, CreatePost changes)
    {
        var post = await GetByIdAsync(id);
        post.Update();
        var updatedPost = changes.Adapt(post);
        await _repository.UpdateAsync();
        return updatedPost.Adapt<PostRes>();
    }

    public async Task DeleteAsync(int id)
    {
        var post = await GetByIdAsync(id);
        await _repository.DeleteAsync(post);
        return;
    }
}
