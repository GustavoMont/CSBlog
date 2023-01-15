using CSBlog.Dtos.User;
using CSBlog.Models;
using CSBlog.Repositories;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Services;

public class UserService
{
    private readonly UserRepository _repository;

    public UserService([FromServices] UserRepository userRepository)
    {
        _repository = userRepository;
    }

    public async Task CreateAsync(CreateUserRequest body)
    {
        if (body.Password != body.ConfirmPassword)
        {
            throw new BadHttpRequestException("As senhas não estão iguais");
        }
        var user = body.Adapt<User>();
        var newUser = await _repository.CreateAsync(user);
        return;
    }
}
