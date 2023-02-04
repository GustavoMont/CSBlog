using System.Security.Claims;
using CSBlog.Dtos;
using CSBlog.Dtos.Posts;
using CSBlog.Exceptions;
using CSBlog.Models;
using CSBlog.Repositories;
using CSBlog.Utils;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Services;

public class PostService : ServiceUtils
{
    private readonly PostRepository _repository;

    public PostService(
        [FromServices] PostRepository repository,
        [FromServices] IHttpContextAccessor httpContext
    ) : base(httpContext)
    {
        _repository = repository;
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

    public async Task<ListResponse<PostRes>> ListAsync(int page = 1, int take = 25)
    {
        HandlePagination(take);
        int skip = GenerateSkip(page, take);
        int? userId = IsAdmin() ? null : GetUserId();
        var posts = await _repository.ListAsync(userId, skip, take);
        var count = await _repository.GetCountAsync();
        ListResponse<PostRes> response =
            new(page, count, take) { Results = posts.Adapt<List<PostRes>>() };
        return response;
    }

    public async Task<ListResponse<PostRes>> ListPublishedAsync(int page = 1, int take = 25)
    {
        HandlePagination(take);
        int skip = GenerateSkip(page, take);
        var posts = await _repository.ListPublishedAsync(skip, take);
        var count = await _repository.GetCountByStatusAsync(PostStatus.PUBLISHED);
        if (count <= 0)
        {
            throw new NotFoundException("Nenhum post foi feito");
        }
        ListResponse<PostRes> response =
            new(page, count, take) { Results = posts.Adapt<List<PostRes>>() };
        return response;
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
