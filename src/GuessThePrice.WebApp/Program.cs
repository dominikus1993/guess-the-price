using GuessThePrice.Core.Grains;
using GuessThePrice.Core.Services;
using GuessThePrice.Infrastructure.Extensions;

using Orleans;
using Orleans.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseOrleans(b =>
{
    b.UseLocalhostClustering();
    b.AddMemoryGrainStorage("games");
    b.UseDashboard(options => options.Port = 8888)
        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(GameGrain).Assembly));
    
});
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/test", () => "xD");
app.UseAuthorization();

app.MapControllers();

app.Run();

