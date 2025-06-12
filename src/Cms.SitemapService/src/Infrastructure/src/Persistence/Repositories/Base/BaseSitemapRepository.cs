using System.Threading;
using System.Threading.Tasks;
using Cms.SitemapService.Domain.Entities;
using Cms.SitemapService.Infrastructure.Persistence.Repositories.Base.Interfaces;
using Cms.SitemapService.Infrastructure.Services.Interfaces;

namespace Cms.SitemapService.Infrastructure.Persistence.Repositories.Base;

internal abstract class BaseSitemapRepository<TSitemap>(
    IDocumentDatabaseService documentDatabaseService
) : IBaseSitemapRepository<TSitemap>
    where TSitemap : Sitemap
{
    protected IDocumentDatabaseService DocumentDatabaseService => documentDatabaseService;

    public async Task InsertAsync(TSitemap sitemap, CancellationToken cancellationToken)
    {
        await DocumentDatabaseService.InsertAsync(sitemap, cancellationToken);
    }
}
