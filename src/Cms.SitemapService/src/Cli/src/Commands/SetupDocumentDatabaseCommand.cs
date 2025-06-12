using System.Threading;
using System.Threading.Tasks;
using Cms.Cli.Commands.Interfaces;
using Cms.SitemapService.Infrastructure.Services.Interfaces;

namespace Cms.SitemapService.Cli.Commands;

public class SetupDocumentDatabaseCommand(IDocumentDatabaseService documentDatabaseService)
    : ICommand
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return documentDatabaseService.SetupCollections();
    }
}
