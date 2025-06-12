using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cms.Cli.Commands.Interfaces;
using Cms.Contracts;
using Cms.SitemapService.Domain.Constants;
using Cms.SitemapService.Domain.Entities;
using Cms.SitemapService.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Cms.SitemapService.Cli.Commands;

public sealed class GeneratePostSitemapsCommand(
    Wolverine.IMessageBus bus,
    IDocumentDatabaseService documentDatabaseService,
    ISiteService siteService,
    ILogger<GeneratePostSitemapsCommand> logger
) : ICommand
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var hasMore = true;
        var offset = 0;
        var limit = SitemapLimits.MaxUrlsPerSitemap;

        var siteUrl = await siteService.GetUrlAsync(cancellationToken);

        while (hasMore)
        {
            var result = await bus.InvokeAsync<BulkSitemapDataResponse>(
                new PostBulkSitemapDataRequest(limit, offset),
                cancellationToken
            );

            var sitemap = CreateSitemap(result, siteUrl, result.Urls.Count == limit);

            await documentDatabaseService.InsertAsync(sitemap, cancellationToken);

            offset += limit;
            hasMore = result.Total > offset;
        }
    }

    private PostSitemap CreateSitemap(
        BulkSitemapDataResponse response,
        string siteUrl,
        bool isLocked
    )
    {
        var urls = response.Urls.Select(x => new SitemapUrl
        {
            EntityId = x.EntityId,
            Location = $"{siteUrl}{x.Path}",
            LastModified = x.LastModified,
            Priority = 0.5,
            ChangeFrequency = ChangeFrequency.daily,
        });

        logger.LogInformation("Sitemap generated with {urlsCount}", urls.Count());

        return new PostSitemap
        {
            Id = Guid.NewGuid(),
            Urls = [.. urls],
            LastModified = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            IsLocked = isLocked,
        };
    }
}
