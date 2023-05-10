using DL;
using Microsoft.EntityFrameworkCore;
//using MySql.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("TestDb:MySql");

builder.Services.AddControllers();
builder.Services.AddAuthentication("cookie").AddCookie("cookie");
builder.Services.AddAuthorization();

builder.Services.AddDbContext<QuantEdDbContext>(options => options.UseMySQL(connectionString));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
