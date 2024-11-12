using DotNetEnv;
using mapa_back;
using mapa_back.Mappers;
using mapa_back.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Env.Load();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string is not set.");
}
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString, o => o.UseNetTopologySuite()), optionsLifetime:ServiceLifetime.Scoped);
builder.Services.AddScoped<IRSPOApiService, RSPOApiService>();
builder.Services.AddScoped<ISchoolsService, SchoolsService>();
builder.Services.AddAutoMapper(typeof(MapToBusinessData));
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCors(options => 
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
 });

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
