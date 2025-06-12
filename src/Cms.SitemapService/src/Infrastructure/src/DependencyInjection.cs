using System.ComponentModel;
using Cms.SitemapService.Infrastructure.Persistence.Repositories;
using Cms.SitemapService.Infrastructure.Persistence.Repositories.Interfaces;
using Cms.SitemapService.Infrastructure.Services;
using Cms.SitemapService.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cms.SitemapService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IHealthChecksBuilder healthChecksBuilder,
        IConfiguration configuration
    )
    {
        healthChecksBuilder.AddInfrastructureHealthChecks(configuration);

        services.AddSingleton<IDocumentDatabaseService, DocumentDatabaseService>();

        services.AddScoped<IPostSitemapRepository, PostSitemapRepository>();

        services.AddScoped<ISiteService, SiteService>();
    }

    public static void AddCliInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDocumentDatabaseService, DocumentDatabaseService>();

        services.AddScoped<ISiteService, SiteService>();
    }

    private static IHealthChecksBuilder AddInfrastructureHealthChecks(
        this IHealthChecksBuilder builder,
        IConfiguration configuration
    )
    {
        return builder;
    }
}
