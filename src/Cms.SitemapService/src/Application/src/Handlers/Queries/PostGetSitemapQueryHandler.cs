using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cms.SitemapService.Application.Contracts.Queries;
using Cms.SitemapService.Application.Handlers.Queries.Interfaces;
using Cms.SitemapService.Domain.Builders;
using Cms.SitemapService.Infrastructure.Persistence.Repositories.Interfaces;

namespace Cms.SitemapService.Application.Handlers.Queries;

internal sealed class PostGetSitemapQueryHandler(IPostSitemapRepository postSitemapRepository)
    : IPostGetSitemapQueryHandler
{
    private const int Limit = 1;

    public async Task<string?> HandleAsync(
        PostGetSitemapQuery request,
        CancellationToken cancellationToken
    )
    {
        var skip = request.SitemapNumber - 1;

        var postSitemaps = await postSitemapRepository.PaginateAsync(
            skip,
            Limit,
            cancellationToken
        );
        var postSitemap = postSitemaps.FirstOrDefault();

        if (postSitemap is null)
        {
            return null;
        }

        var builder = new SitemapBuilder();

        builder.AddUrls(postSitemap);

        return builder.GetResult();
    }
}
