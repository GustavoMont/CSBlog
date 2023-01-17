using CSBlog.Dtos.Token;
using CSBlog.Dtos.User;
using CSBlog.Models;
using CSBlog.Repositories;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Services;

public class UserService
{
    private readonly UserRepository _repository;
    private readonly TokenService _tokenService;

    public UserService(
        [FromServices] UserRepository userRepository,
        [FromServices] TokenService tokenService
    )
    {
        _repository = userRepository;
        _tokenService = tokenService;
    }

    private AuthToken GenerateToken(User user)
    {
        AuthToken token = new() { Access = _tokenService.GenerateToken(user) };
        return token;
    }

    public void ComparePasswords(string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            throw new BadHttpRequestException("As senhas não estão iguais");
        }
    }

    public async Task UserAlreadyExists(string email)
    {
        var user = await GetByEmailAsync(email);
        if (user is not null)
        {
            throw new BadHttpRequestException("Usuário já cadastrado");
        }
    }

    public async Task<AuthToken> LoginAsync(LoginReq credential)
    {
        var user = await GetByEmailAsync(credential.Email);
        if (user is null)
        {
            throw new BadHttpRequestException("Usuário ou senha incorretos");
        }
        var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(credential.Password, user.Password);
        if (!isPasswordCorrect)
        {
            throw new BadHttpRequestException("Usuário ou senha incorretos");
        }
        return GenerateToken(user);
    }

    public async Task<UserResponse> CreateTeamUserAsync(CreateTeamUser body)
    {
        await UserAlreadyExists(body.Email);
        ComparePasswords(body.Password, body.ConfirmPassword);
        var existUser = await GetByEmailAsync(body.Email);
        if (existUser is not null) { }
        ComparePasswords(body.Password, body.ConfirmPassword);
        var user = body.Adapt<User>();
        if (Enum.IsDefined(typeof(UserType), body.UserType))
        {
            throw new BadHttpRequestException("Tipo de usuário não existente");
        }
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var newUser = await _repository.CreateAsync(user);
        return newUser.Adapt<UserResponse>();
    }

    public async Task<AuthToken> CreateAsync(CreateUserRequest body)
    {
        await UserAlreadyExists(body.Email);
        ComparePasswords(body.Password, body.ConfirmPassword);
        var user = body.Adapt<User>();
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var newUser = await _repository.CreateAsync(user);
        return GenerateToken(newUser);
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
