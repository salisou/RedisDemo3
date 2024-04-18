using Microsoft.EntityFrameworkCore;
using RedisDemo3.Cache;
using RedisDemo3.DBContext;
using RedisDemo3.Extentions;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ottieni la stringa di connessione per MySQL dal file di configurazione
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// Ottieni la stringa di connessione per Redis dal file di configurazione
var cacheRedis = builder.Configuration.GetConnectionString("RedisCache");

// Controlla se una delle stringhe di connessione è nulla o vuota
if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(cacheRedis))
{
    // Se una delle stringhe di connessione è nulla o vuota, lancia un'eccezione con un messaggio descrittivo
    throw new InvalidOperationException("La stringa di connessione MySQL o Redis non può essere null o vuota.");
}

// Se entrambe le stringhe di connessione sono valide, registra il DbContext per MySQL nel servizio
builder.Services.AddDbContext<DbContextClass>(options =>
    options.UseMySQL(connectionString));

// Registra il servizio di caching Redis nel servizio utilizzando la stringa di connessione Redis
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = cacheRedis);


// Register the CacheService as the implementation of ICacheService.
// This makes an instance of CacheService available throughout the application
// wherever ICacheService is injected as a dependency. The Scoped lifetime means
// that a single instance of CacheService is used for each HTTP request.
builder.Services.AddScoped<ICacheService, CacheService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigration(); // per fare la migrazione (EntityFrameworkCore\Add-Migration initialCreate -startupProject RedisDemo3 -Project RedisDemo3)
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
