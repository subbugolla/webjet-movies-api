using MoviesApi.Common;
using MoviesApi.Controllers;
using MoviesApi.Interfaces;
using MoviesApi.Middleware;
using MoviesApi.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog

builder.Services.AddSerilog((context, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration);
});

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddScoped<ICinemaworldService, CinemaworldService>();
builder.Services.AddScoped<IFilmworldService, FilmworldService>();
builder.Services.AddScoped<IMovieWorldController, MovieWorldController>();
builder.Services.AddScoped<HeaderMiddleware>();
builder.Services.AddScoped<IServiceClient, ServiceClient>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(ApiConstants.CinemaworldApiUrl, (provider, client) =>
{
    client.BaseAddress = new Uri(builder.Configuration[ApiConstants.CinemaworldApiUrl]);
}).AddHttpMessageHandler<HeaderMiddleware>();
builder.Services.AddHttpClient(ApiConstants.FilmworldApiUrl, (provider, client) =>
{
    client.BaseAddress = new Uri(builder.Configuration[ApiConstants.FilmworldApiUrl]);
}).AddHttpMessageHandler<HeaderMiddleware>();

builder.Services.AddDistributedMemoryCache();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:3000", "http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
