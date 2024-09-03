using Grpc.Net.Client;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Net.Http;

var serviceName = "dice-server";
var serviceVersion = "1.0.0";

var builder = WebApplication.CreateBuilder(args);

var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator; // TODO: Remove this line in production, replace with safe certificate handling and validation

var grpcChannelOptions = new GrpcChannelOptions
{
    HttpClient = new HttpClient(handler)
};

var grpcChannel = GrpcChannel.ForAddress("https://localhost:4317", grpcChannelOptions);

builder.Services.AddOpenTelemetry()
    .ConfigureResource(Resource => Resource.AddService(
        serviceName: serviceName,
        serviceVersion: serviceVersion))
    .WithTracing(tracing => tracing
        .AddSource(serviceName)
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter()
        .AddOtlpExporter(options => {
            options.Endpoint = new Uri("https://localhost:4317");
            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        }))
    .WithMetrics(metrics => metrics
        .AddMeter(serviceName)
        .AddConsoleExporter()
        .AddOtlpExporter(options => {
            options.Endpoint = new Uri("https://localhost:4317");
            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        }));

builder.Logging.AddOpenTelemetry(options => {
    options
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
            serviceName: serviceName,
            serviceVersion: serviceVersion))
        .AddConsoleExporter()
        .AddOtlpExporter(options => {
            options.Endpoint = new Uri("https://localhost:4317");
            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        });
});

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();

/*
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);


// OTEL Setup
const string serviceName = "roll-dice";

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddConsoleExporter();
});
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter());

var app = builder.Build();

string HandleRollDice([FromServices]ILogger<Program> logger, string? player)
{
    var result = RollDice();

    if (string.IsNullOrEmpty (player))
    {
        logger.LogInformation("Anonymous player rolled a dice with result: {result}", result);
    }
    else
    {
        logger.LogInformation("{player} rolled a dice with result: {result}", player, result);
    }

    return result.ToString(CultureInfo.InvariantCulture);
}

int RollDice()
{
    return Random.Shared.Next(1, 7);
}

app.MapGet("/rolldice/{player?}", HandleRollDice);

app.Run();
*/