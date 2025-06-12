using Cms.SitemapService.Application.Handlers.Commands;
using Cms.SitemapService.Application.Handlers.Commands.Interfaces;
using Cms.SitemapService.Application.Handlers.Queries;
using Cms.SitemapService.Application.Handlers.Queries.Interfaces;
using Cms.SitemapService.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cms.SitemapService.Application;

public static class DependencyInjection
{
    public static void AddApplication(
        this IServiceCollection services,
        IHealthChecksBuilder healthChecksBuilder,
        IConfiguration configuration
    )
    {
        services.AddInfrastructure(healthChecksBuilder, configuration);

        services.AddScoped<
            IPostSitemapUpsertUrlCommandHandler,
            PostSitemapUpsertUrlCommandHandler
        >();
        services.AddScoped<
            IPostSitemapDeleteUrlCommandHandler,
            PostSitemapDeleteUrlCommandHandler
        >();
        services.AddScoped<IPostGetSitemapIndexQueryHandler, PostGetSitemapIndexQueryHandler>();
        services.AddScoped<IPostGetSitemapQueryHandler, PostGetSitemapQueryHandler>();
    }
}
