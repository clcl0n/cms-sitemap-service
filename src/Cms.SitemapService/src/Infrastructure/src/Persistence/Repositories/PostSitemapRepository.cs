using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cms.SitemapService.Domain.Entities;
using Cms.SitemapService.Infrastructure.Persistence.Repositories.Base;
using Cms.SitemapService.Infrastructure.Persistence.Repositories.Interfaces;
using Cms.SitemapService.Infrastructure.Services.Interfaces;

namespace Cms.SitemapService.Infrastructure.Persistence.Repositories;

internal sealed class PostSitemapRepository(IDocumentDatabaseService documentDatabaseService)
    : BaseSitemapRepository<PostSitemap>(documentDatabaseService),
        IPostSitemapRepository
{
    public Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return DocumentDatabaseService.CountAsync<PostSitemap>(cancellationToken);
    }

    public Task<List<PostSitemap>> PaginateAsync(
        int skip,
        int limit,
        CancellationToken cancellationToken
    )
    {
        return DocumentDatabaseService.PaginateAsync<PostSitemap>(skip, limit, cancellationToken);
    }

    public Task InsertEntityUrlAsync(SitemapUrl url, CancellationToken cancellationToken)
    {
        return DocumentDatabaseService.InsertEntityUrlAsync<PostSitemap>(url, cancellationToken);
    }

    public Task DeleteEntityUrlsAsync(Guid entityId, CancellationToken cancellationToken)
    {
        return DocumentDatabaseService.DeleteEntityUrlsAsync<PostSitemap>(
            entityId,
            cancellationToken
        );
    }
}
