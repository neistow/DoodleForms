using DoodleForms.Identity.Common;
using DoodleForms.Identity.Data;
using DoodleForms.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

services.AddDbContext<ApplicationDbContext>(o =>
    o.UseNpgsql(configuration.GetConnectionString("postgres"))
);

// Asp Net Identity Config
services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Identity Server Config
services
    .AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddAspNetIdentity<ApplicationUser>();

var app = builder.Build();

app.UseRouting();
app.UseIdentityServer();

app.UseEndpoints(endpoints => { endpoints.MapGet("/ping", context => context.Response.WriteAsync("Pong")); });

app.Run();