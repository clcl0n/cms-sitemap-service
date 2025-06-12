using System.Threading;
using System.Threading.Tasks;
using Cms.Contracts;
using Cms.SitemapService.Application.Handlers.Commands.Interfaces;
using Cms.SitemapService.Infrastructure.Persistence.Repositories.Interfaces;

namespace Cms.SitemapService.Application.Handlers.Commands;

internal sealed class PostSitemapDeleteUrlCommandHandler(
    IPostSitemapRepository postSitemapRepository
) : IPostSitemapDeleteUrlCommandHandler
{
    public Task HandleAsync(
        PostSitemapDeleteUrlRequest request,
        CancellationToken cancellationToken
    )
    {
        return postSitemapRepository.DeleteEntityUrlsAsync(request.EntityId, cancellationToken);
    }
}
