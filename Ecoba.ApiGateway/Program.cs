using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration(webHost => webHost.AddJsonFile(Path.Combine("ocelot.json"), false, true).AddEnvironmentVariables());

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options => options.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
}

builder.Services.AddCors(options => options.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddOcelot().AddConsul().AddConfigStoredInConsul();

var app = builder.Build();

// app.UseAuthentication();

// app.UseAuthorization();

app.UseCors();

app.UseOcelot().Wait();

app.MapGet("/", () => "Ecoba Api Gateway!");

app.Run();
