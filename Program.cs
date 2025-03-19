using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ZooKeepers.Data;
using ZooKeepers.Models;
using ZooKeepers.Constants;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ZooDbContext>(options => options.UseSqlite(connectionString));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddLogging();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    ZooKeepersSeed.createEnclosures(services);
    ZooKeepersSeed.SeedAnimals(services);
}

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
