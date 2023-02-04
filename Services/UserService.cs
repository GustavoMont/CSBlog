using CSBlog.Dtos;
using CSBlog.Dtos.Email;
using CSBlog.Dtos.Token;
using CSBlog.Dtos.User;
using CSBlog.Exceptions;
using CSBlog.Models;
using CSBlog.Repositories;
using CSBlog.Utils;
using MailKit.Net.Smtp;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace CSBlog.Services;

public class UserService : ServiceUtils
{
    private readonly UserRepository _repository;
    private readonly TokenService _tokenService;
    private readonly EmailService _emailService;

    public UserService(
        [FromServices] UserRepository userRepository,
        [FromServices] TokenService tokenService,
        [FromServices] EmailService emailService,
        [FromServices] IHttpContextAccessor httpContextAccessor
    ) : base(httpContextAccessor)
    {
        _repository = userRepository;
        _tokenService = tokenService;
        _emailService = emailService;
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
        if (!Enum.IsDefined(typeof(UserType), body.UserType))
        {
            throw new BadHttpRequestException("Tipo de usuário não existente");
        }
        var user = body.Adapt<User>();
        var newUser = await _repository.CreateAsync(user);
        return newUser.Adapt<UserResponse>();
    }

    public async Task<AuthToken> CreateAsync(CreateUserRequest body)
    {
        await UserAlreadyExists(body.Email);
        ComparePasswords(body.Password, body.ConfirmPassword);
        var user = body.Adapt<User>();
        var newUser = await _repository.CreateAsync(user);
        return GenerateToken(newUser);
    }

    public async Task<ListResponse<UserResponse>> ListAsync(int page = 1, int take = 25)
    {
        HandlePagination(take);
        var count = await _repository.GetCountAsync();
        if (count <= 0)
        {
            throw new HttpRequestException("Nenhum usuário cadastrado");
        }
        var skip = GenerateSkip(page, take);
        var users = await _repository.ListAsync(skip, take);
        ListResponse<UserResponse> response =
            new(page, count, take) { Results = users.Adapt<List<UserResponse>>() };
        return response;
    }

    private async Task<User> GetByEmailAsync(string email)
    {
        var user = await _repository.GetByEmailAsync(email);
        return user;
    }

    private async Task<User> GetByIdAsync(int id, bool tracking = true)
    {
        var user = await _repository.GetById(id, tracking);
        return user;
    }

    public async Task<UserResponse> GetOneAsync(int id)
    {
        var user = await GetByIdAsync(id, false);
        if (user is null)
        {
            throw new HttpRequestException("Este usuário não existe");
        }
        return user.Adapt<UserResponse>();
    }

    public async Task SendResetPasswordEmailAsync(string email)
    {
        var user = await GetByEmailAsync(email);
        if (user is null)
        {
            throw new NotFoundException("Usuário não cadatrado");
        }
        var fullName = $"{user.Name} {user.LastName}";
        _emailService.SendEmail(
            new SendEmailReq
            {
                Content =
                    $"<h1>Mude sua senha</h1>\nPara mudar sua senha acesse {Environment.GetEnvironmentVariable("FRONT_DOMAIN")}/reset-password/{_tokenService.GenerateResetPasswordToken(user.Id)}",
                Email = user.Email,
                FullName = $"{user.Name} {user.LastName}",
                Subject = "Redefinir senha"
            }
        );
    }

    public async Task ResetPasswordAsync(ResetPasswordReq body)
    {
        if (!_tokenService.IsTokenValid(body.Token))
        {
            throw new BadHttpRequestException("Token inválido");
        }
        ComparePasswords(body.Password, body.ConfirmPassword);
        int userId = _tokenService.GetUserId(body.Token);
        var user = await _repository.GetById(userId);
        user.Password = body.Password;
        await _repository.UpdateAsync();
    }

    public async Task UpdateAsync(int id, UpdateUserReq changes)
    {
        var user = await GetByIdAsync(id);
        if (user is null)
        {
            throw new NotFoundException("Usuário não encontrado");
        }
        if (user.Id != GetUserId())
        {
            throw new ForbiddenException();
        }
        if (user.Email != changes.Email)
        {
            await UserAlreadyExists(changes.Email);
        }
        user.Update();
        var updatedUser = changes.Adapt(user);
        await _repository.UpdateAsync();
    }
}
