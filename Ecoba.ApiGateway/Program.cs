using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ecoba.ApiGateway.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration(webHost => webHost.AddJsonFile(Path.Combine("ocelot.json"), false, true).AddEnvironmentVariables());

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options => options.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
}

builder.Services.AddCors(options => options.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddOcelot().AddConsul().AddConfigStoredInConsul();

//var authenticationProviderKey = "TestKey";
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
 {
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
         ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
     };
 });

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors();

app.UseOcelot().Wait();

app.MapGet("/", () => "Ecoba Api Gateway!");

app.Run();
