using System.Threading;
using System.Threading.Tasks;
using Cms.Contracts;
using Cms.SitemapService.Infrastructure.Services.Interfaces;
using Wolverine;

namespace Cms.SitemapService.Infrastructure.Services;

internal sealed class SiteService(IMessageBus bus) : ISiteService
{
    public async Task<string> GetUrlAsync(CancellationToken cancellationToken)
    {
        var response = await bus.InvokeAsync<SiteGetResponse>(
            new SiteGetRequest(),
            cancellationToken
        );

        return response.Url;
    }
}
