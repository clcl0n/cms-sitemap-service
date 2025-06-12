using System.Threading;
using System.Threading.Tasks;
using Cms.SitemapService.Application.Contracts.Queries;
using Cms.SitemapService.Application.Handlers.Queries.Interfaces;
using Cms.SitemapService.Domain.Builders;
using Cms.SitemapService.Infrastructure.Persistence.Repositories.Interfaces;

namespace Cms.SitemapService.Application.Handlers.Queries;

internal sealed class PostGetSitemapIndexQueryHandler(IPostSitemapRepository postSitemapRepository)
    : IPostGetSitemapIndexQueryHandler
{
    public async Task<string?> HandleAsync(
        PostGetSitemapIndexQuery request,
        CancellationToken cancellationToken
    )
    {
        var count = await postSitemapRepository.CountAsync(cancellationToken);

        if (count == 0)
        {
            return null;
        }

        return GetSitemapIndex(request.SitemapUrlTemplate, count);
    }

    private static string GetSitemapIndex(string sitemapUrlTemplate, long count)
    {
        var builder = new SitemapIndexBuilder();

        builder.AddSitemaps(sitemapUrlTemplate, count);

        return builder.GetResult();
    }
}
