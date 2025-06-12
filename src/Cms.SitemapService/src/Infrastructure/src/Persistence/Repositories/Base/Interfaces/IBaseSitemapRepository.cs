using System.Threading;
using System.Threading.Tasks;
using Cms.SitemapService.Domain.Entities;

namespace Cms.SitemapService.Infrastructure.Persistence.Repositories.Base.Interfaces;

public interface IBaseSitemapRepository<TSitemap>
    where TSitemap : Sitemap
{
    Task InsertAsync(TSitemap sitemap, CancellationToken cancellationToken);
}
