using API.Extensions;
using API.Middlewares;
using Core;
using Core.Entities.Identity;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAPIService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

//app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// init or update database
using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;
var context = service.GetRequiredService<ApplicationDbContext>();
var contextIdentity = service.GetRequiredService<AppIdentityDbContext>();
var userManager = service.GetRequiredService<UserManager<AppUser>>();
var logger = service.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync(); 
    await contextIdentity.Database.MigrateAsync();
    await AppIdentityDbContextSeed.SeedUserAync(userManager);
    //await ApplicationDbContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration.");
}


app.Run();
