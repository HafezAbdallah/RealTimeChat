using Microsoft.EntityFrameworkCore;
using RealTimeChat;
using RealTimeChat.Core.Repos;
using RealTimeChat.Core.Services.Implmentations;
using RealTimeChat.Core.Services.Interfaces;
using RealTimeChat.Hubs;
using RealTimeChat.Infra.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();


builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IUsermanagementRepo, UserManagementRepo>();
builder.Services.AddSingleton<IConnectionManager, ConnectionManager>(); 


builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=../RealTimeChat.Infra/ChatDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(options => options
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              );

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
