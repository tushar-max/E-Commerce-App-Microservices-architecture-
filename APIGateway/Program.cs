using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json");
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddOcelot().AddEureka();

var app = builder.Build();
app.UseOcelot().Wait();
app.MapControllers();
//app.MapGet("/", () => "Hello World!");
//app.UseOcelot();

app.Run();
