using System.Threading;
using System.Threading.Tasks;
using Cms.Contracts;
using Cms.SitemapService.Application.Handlers.Commands.Interfaces;
using Wolverine.Attributes;

namespace Cms.SitemapService.Api.MessageHandlers;

[WolverineHandler]
public sealed class PostSitemapDeleteUrlRequestMessageHandler(
    IPostSitemapDeleteUrlCommandHandler commandHandler
)
{
    public Task HandleAsync(
        PostSitemapDeleteUrlRequest request,
        CancellationToken cancellationToken
    )
    {
        return commandHandler.HandleAsync(
            new PostSitemapDeleteUrlRequest(request.EntityId),
            cancellationToken
        );
    }
}
