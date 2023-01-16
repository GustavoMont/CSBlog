using CSBlog.Data;
using Microsoft.EntityFrameworkCore;
using CSBlog.Repositories;
using CSBlog.Services;
using CSBlog.Controllers;
using dotenv.net;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adicionando a minha classe de contexto na API
//using using IFBeaty.Data;
builder.Services.AddDbContext<Context>(
    options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("Connection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Connection"))
        )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
