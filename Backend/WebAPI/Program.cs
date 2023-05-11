using DL;
using Microsoft.EntityFrameworkCore;
using WebAPI.Services;
using WebAPI.Services.Abstractions;

//using MySql.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("TestDb:MySql");

builder.Services.AddControllers();
builder.Services.AddAuthentication("cookie").AddCookie("cookie");
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<QuantEdDbContext>(options => options.UseMySQL(connectionString));
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// to be used in the future
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();