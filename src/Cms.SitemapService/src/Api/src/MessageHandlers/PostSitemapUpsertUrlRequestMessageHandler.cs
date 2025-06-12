using System.Threading;
using System.Threading.Tasks;
using Cms.Contracts;
using Cms.SitemapService.Application.Handlers.Commands.Interfaces;
using Wolverine.Attributes;

namespace Cms.SitemapService.Api.MessageHandlers;

[WolverineHandler]
public sealed class PostSitemapUpsertUrlRequestMessageHandler(
    IPostSitemapUpsertUrlCommandHandler commandHandler
)
{
    public async Task HandleAsync(
        PostSitemapUpsertUrlRequest request,
        CancellationToken cancellationToken
    )
    {
        await commandHandler.HandleAsync(
            new PostSitemapUpsertUrlRequest(request.Id, request.Path, request.LastModified),
            cancellationToken
        );
    }
}
