using MongoDB.Driver;
using Terminal42.Models;
using Terminal42.Services.Auth;
using Terminal42.Repositories.Users;
using Terminal42.Services.Users;
using Terminal42.Hubs;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

// Configure MongoDBSettings using appsettings.json
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

// Register MongoClient as singleton
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>();
    return new MongoClient(settings?.ConnectionString);
});

// OPTIONAL: Register your MongoDB database instance
builder.Services.AddScoped(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>();
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings?.DatabaseName);
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
// Configure CORS:
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:5173"); // Frontend server port
    });
});

var app = builder.Build();

app.UseCors();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.MapGet(
    "/mongo-test",
    async (IMongoDatabase db) =>
    {
        try
        {
            // Try to list collections (will throw if not connected)
            var collections = await db.ListCollectionNames().ToListAsync();
            return Results.Ok(new { connected = true, collections });
        }
        catch (Exception ex)
        {
            return Results.Problem("MongoDB connection failed: " + ex.Message);
        }
    }
);

app.MapHub<ChatHub>("/hubs/chat");
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
