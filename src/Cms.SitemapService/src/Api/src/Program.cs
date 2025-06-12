using System.Text.Json.Serialization;
using Cms.Shared.Setups;
using Cms.SitemapService.Api.Extensions;
using Cms.SitemapService.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Cms.SitemapService.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ConfigureOtel();

        builder.Services.ConfigureOtel();

        var healthChecksBuilder = builder.Services.AddHealthChecks();

        builder.Services.AddApplication(healthChecksBuilder, builder.Configuration);

        builder
            .Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    // show enum value in swagger.
                    new JsonStringEnumConverter()
                );
            });

        builder.Services.SetupApiConfiguration(builder.Configuration);

        healthChecksBuilder.ConfigureHealthCheck(builder.Configuration);

        builder.Services.AddOpenApi();

        builder.Services.ConfigureWolverine(builder.Configuration);

        using var app = builder.Build();

        app.MapOpenApi();

        // app.UseExceptionHandler();
        app.UseStatusCodePages();

        app.UseHealthCheck();

        app.MapControllers();

        app.MapScalarApiReference();

        app.Run();
    }
}
