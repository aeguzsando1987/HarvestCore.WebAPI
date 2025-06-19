using HarvestCore.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;
using HarvestCore.WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Lee la cadena de conexión de la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//2. Registra ApplicationDbContext para injeccion de dependencias, usa SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(connectionString)
);

// 3. Registra servicios de controladores y Swagger/con soporte para OpenAPI
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra tus repositorios aquí
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddScoped<ICrewRepository, CrewRepository>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// 4. Aplica cualquier migracion pendiente y crea la base de datos si no existe al arrancar la aplicación
// Esto asegura que la base de datos sea creada correctamente y las migraciones se ejecuten automáticamente
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        if(dbContext.Database.IsRelational())
        {
            dbContext.Database.Migrate(); // Aplica migraciones pendientes. Crea la BD si no existe
        }
     
    }
    catch (Exception ex)
    {
        // TODO: Logear error
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
} 

// 5. Configura el pipeline de peticiones http
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Removida pues el proyecto fue creado con --no-https
app.UseAuthorization(); // Configura autorización JWT
app.MapControllers(); // Configura controladores

app.Run(); // Inicia el servidor
