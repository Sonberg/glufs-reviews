using Azure.Identity;
using Glufs.Reviews.Api.Extensions;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

