using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Glufs.Reviews.Api.Extensions;
using Glufs.Reviews.Api.Options;
using Glufs.Reviews.Application;
using Glufs.Reviews.Domain;
using Glufs.Reviews.Domain.Klaviyo;
using Glufs.Reviews.Domain.Klaviyo.Models;
using Glufs.Reviews.Infrastructure;
using Glufs.Reviews.Infrastructure.Options;
using Glufs.Reviews.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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

builder.Services
    .AddOptions<ShopifyAdminOptions>()
    .Bind(builder.Configuration.GetSection(ShopifyAdminOptions.SectionName))
    .ValidateDataAnnotations();


builder.Services.AddSingleton(sp =>
{
    if (builder.Environment.IsDevelopment())
    {
        return new ServiceBusClient("glufs-dev-sbus.servicebus.windows.net", new DefaultAzureCredential());
    }
    else
    {
        return new ServiceBusClient(sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value.ConnectionString!);
    }

});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureInfrastructure(builder.Configuration);
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

app.MapGet("/test-klaviyo", async (IKlaviyo klaviyo) =>
{
    await klaviyo.AskForReview(new AskForReviewRequest
    {
        Email = "per.sonberg+nyemail@gmail.com",
        Phone = null,
        ReviewRequestId = Guid.Empty.ToString(),
        OrderId = Guid.Empty.ToString()
    });
});

app.Run();

