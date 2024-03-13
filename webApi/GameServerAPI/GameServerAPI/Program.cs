 using DataAccess;
using DataAccess.Repositories;
using Domain;
using Domain.Interfaces;
using GameServerAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(routes => routes.EnableDetailedErrors = true);

builder.Services.AddDbContext<ApplicationContext>(option =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database");
    option.UseSqlServer(connectionString,
                        b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)); 
});

#region Repositories
builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient<IPlayerRepository, PlayerRepository>();
builder.Services.AddTransient<IGameRoundRepository, GameRoundRepository>();
builder.Services.AddTransient<IPlayerRoundRepository, PlayerRoundRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
#endregion

#region Services
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IGameRoundService, GameRoundService>();
builder.Services.AddScoped<IPlayerRoundService, PlayerRoundService>();
#endregion

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapHub<GameServerHub>("/gameServerHub");

app.UseAuthorization();
app.UseCors(builder =>
                        builder.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod());


app.MapControllers();

app.Run();
