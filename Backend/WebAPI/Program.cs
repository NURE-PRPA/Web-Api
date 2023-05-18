using DL;
using Microsoft.EntityFrameworkCore;
using WebAPI.Services;
using WebAPI.Services.Abstractions;

//using MySql.EntityFrameworkCore;
var angularApp = "Angular web app";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Angular web app", option =>
    {
        option
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("MyLittleSnus");
var connectionString = builder.Configuration.GetConnectionString("Sentinel");
//var connectionString = builder.Configuration.GetConnectionString("Anastasiia");
//var connectionString = builder.Configuration.GetConnectionString("VasyokMcT4wer");
//var connectionString = builder.Configuration.GetConnectionString("NoNamesNoGames");
//var connectionString = builder.Configuration.GetConnectionString("DeGrand");

builder.Services.AddControllers();
builder.Services.AddAuthentication("cookie").AddCookie("cookie");
builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<QuantEdDbContext>(options => options.UseMySQL(connectionString));
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserAttemptService, UserAttemptService>();

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseRouting();

app.UseCors(angularApp);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();