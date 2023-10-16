using Azure.Identity;
using Glufs.Reviews.Api.Extensions;
using Glufs.Reviews.Application;
using Glufs.Reviews.Domain;
using Glufs.Reviews.Infrastructure;
using Glufs.Reviews.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

if (builder.Configuration.TryGetValue<Uri>("KeyVaultUri", out var keyVaultUri))
{
    builder.Configuration.AddAzureKeyVault(keyVaultUri!, new DefaultAzureCredential());
}

if (builder.Configuration.TryGetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING", out var connectionString))
{
    builder.Services.AddApplicationInsightsTelemetry(opt =>
    {
        opt.ConnectionString = connectionString;
    });
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureInfrastructure();
builder.Services.ConfigureDomain();
builder.Services.ConfigureJobs();
builder.Services.ConfigureApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

