using System.Threading;
using System.Threading.Tasks;

namespace Cms.SitemapService.Infrastructure.Services.Interfaces;

public interface ISiteService
{
    Task<string> GetUrlAsync(CancellationToken cancellationToken);
}
