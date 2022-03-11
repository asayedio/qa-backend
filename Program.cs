using DbUp;
using QandA.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDataRepository, DataRepository>();
// Add MemoryCache 
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IQuestionCache, QuestionCache>();

ConfigurationManager configuration = builder.Configuration;

// Authenticate with Auth0
builder.Services.AddAuthentication(options => { 
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => { 
    options.Authority = configuration["Auth0:Authority"];
    options.Audience = configuration["Auth0:Audience"];
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // For managing migrations using DbUp
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    EnsureDatabase.For.SqlDatabase(connectionString);
    // Create and confgure an instance of the DbUp upgrader
    var upgrader = DeployChanges.To.SqlDatabase(connectionString, null)
        .WithScriptsEmbeddedInAssembly(System.Reflection.Assembly
        .GetExecutingAssembly())
        .WithTransaction()
        .Build();
    // Do a database migration if there are any pending SQL Scripts
    /* We are using the IsUpgradeRequired method in the DbUp upgrade to check whether there are any pending SQL Scripts, and using the PerformUpgrade method to do the actual migration **/
    if (upgrader.IsUpgradeRequired())
        upgrader.PerformUpgrade();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
