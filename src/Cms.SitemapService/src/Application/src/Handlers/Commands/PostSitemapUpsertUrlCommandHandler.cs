using System.Threading;
using System.Threading.Tasks;
using Cms.Contracts;
using Cms.SitemapService.Application.Handlers.Commands.Interfaces;
using Cms.SitemapService.Domain.Constants;
using Cms.SitemapService.Domain.Entities;
using Cms.SitemapService.Infrastructure.Persistence.Repositories.Interfaces;
using Cms.SitemapService.Infrastructure.Services.Interfaces;

namespace Cms.SitemapService.Application.Handlers.Commands;

internal sealed class PostSitemapUpsertUrlCommandHandler(
    IPostSitemapRepository postSitemapRepository,
    ISiteService siteService
) : IPostSitemapUpsertUrlCommandHandler
{
    public async Task HandleAsync(
        PostSitemapUpsertUrlRequest request,
        CancellationToken cancellationToken
    )
    {
        var siteUrl = await siteService.GetUrlAsync(cancellationToken);

        var sitemapUrl = new SitemapUrl
        {
            EntityId = request.Id,
            Location = $"{siteUrl}/{request.Path}",
            LastModified = request.LastModified,
            ChangeFrequency = ChangeFrequency.daily,
            Priority = 0.5,
        };

        await postSitemapRepository.InsertEntityUrlAsync(sitemapUrl, cancellationToken);
    }
}
