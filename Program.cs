using DbUp;
using QandA.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDataRepository, DataRepository>();
var app = builder.Build();

ConfigurationManager configuration = builder.Configuration;

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

app.UseAuthorization();

app.MapControllers();

app.Run();
