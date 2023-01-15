using CSBlog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Services;

public class UserService
{
    private readonly UserRepository _repository;

    public UserService([FromServices] UserRepository userRepository)
    {
        _repository = userRepository;
    }
}
