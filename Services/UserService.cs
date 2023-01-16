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
        if (!Enum.IsDefined(typeof(UserType), body.UserType))
        {
            throw new BadHttpRequestException("Tipo de usuário não cadastrado");
        }
        var user = body.Adapt<User>();
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var newUser = await _repository.CreateAsync(user);
        return;
    }

    public async Task<List<UserResponse>> ListAsync()
    {
        var users = await _repository.ListAsync();
        if (users.Count <= 0)
        {
            throw new HttpRequestException("Nenhum usuário cadastrado");
        }
        var res = users.Adapt<List<UserResponse>>();
        return res;
    }

    private async Task<User> GetByEmailAsync(string email)
    {
        var user = await _repository.GetByEmailAsync(email);
        return user;
    }

    private async Task<User> GetByIdAsync(int id)
    {
        var user = await _repository.GetById(id);
        return user;
    }

    public async Task<UserResponse> GetOneAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user is null)
        {
            throw new HttpRequestException("Este usuário não existe");
        }
        return user.Adapt<UserResponse>();
    }
}
