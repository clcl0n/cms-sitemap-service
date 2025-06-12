using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cms.SitemapService.Domain.Entities;
using Cms.SitemapService.Infrastructure.Persistence.Repositories.Base.Interfaces;

namespace Cms.SitemapService.Infrastructure.Persistence.Repositories.Interfaces;

public interface IPostSitemapRepository : IBaseSitemapRepository<PostSitemap>
{
    Task<long> CountAsync(CancellationToken cancellationToken);

    Task<List<PostSitemap>> PaginateAsync(int skip, int limit, CancellationToken cancellationToken);

    Task InsertEntityUrlAsync(SitemapUrl url, CancellationToken cancellationToken);

    Task DeleteEntityUrlsAsync(Guid entityId, CancellationToken cancellationToken);
}
