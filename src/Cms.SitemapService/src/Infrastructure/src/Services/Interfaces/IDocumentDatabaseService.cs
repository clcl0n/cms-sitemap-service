using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cms.SitemapService.Domain.Entities;

namespace Cms.SitemapService.Infrastructure.Services.Interfaces;

public interface IDocumentDatabaseService
{
    Task<long> CountAsync<TSitemap>(CancellationToken cancellationToken)
        where TSitemap : Sitemap;

    Task<List<TSitemap>> PaginateAsync<TSitemap>(
        int skip,
        int limit,
        CancellationToken cancellationToken
    )
        where TSitemap : Sitemap;

    Task InsertAsync<TSitemap>(TSitemap sitemap, CancellationToken cancellationToken)
        where TSitemap : Sitemap;

    Task InsertEntityUrlAsync<TSitemap>(SitemapUrl sitemapUrl, CancellationToken cancellationToken)
        where TSitemap : Sitemap;

    Task DeleteEntityUrlsAsync<TSitemap>(Guid entityId, CancellationToken cancellationToken)
        where TSitemap : Sitemap;

    Task SetupCollections();
}
