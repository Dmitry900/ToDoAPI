using System.Text;
using ToDoAPI.Extensions;
using ToDoAPI.Services;
using ToDoAPI.Services.Interfaces;
using ToDoAPI.JWT.Model;
using ToDoAPI.JWT.Services;
using ToDoAPI.JWT.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoAPIContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoAPIContext") ?? throw new InvalidOperationException("Connection string 'ToDoAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddAuthentication();


IConfiguration configuration = builder.Configuration;

IConfigurationSection settingsSection = configuration.GetSection("EncryptionKey");
EncryptionKey settings = settingsSection.Get<EncryptionKey>();
byte[] signingKey = Encoding.UTF8.GetBytes(settings.Key);

builder.Services.Configure<EncryptionKey>(settingsSection);

builder.Services.AddAuthentication(signingKey);


builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();



var app = builder.Build();







// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();

}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
