using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Charger le fichier ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Ajouter Ocelot
builder.Services.AddOcelot();

var app = builder.Build();

// Activer Ocelot
await app.UseOcelot();

app.Run();