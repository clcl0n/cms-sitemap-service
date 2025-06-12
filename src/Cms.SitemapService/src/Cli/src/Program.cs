using System.Threading.Tasks;
using Cms.Cli;
using Cms.Cli.Extensions;
using Cms.Shared.Setups;
using Cms.SitemapService.Cli.Commands;
using Cms.SitemapService.Infrastructure;
using Wolverine;

namespace Cms.SitemapService.Cli;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = CliBuilder.CreateCliBuilder(args);

        builder.Services.AddWolverine(options =>
        {
            options.UseRabbitMq(builder.Configuration);
        });

        builder.Services.AddCliInfrastructure();

        builder.Services.AddCommand<GeneratePostSitemapsCommand>("generate-post-sitemaps");
        builder.Services.AddCommand<SetupDocumentDatabaseCommand>("setup-document-database");

        await CliBuilder.RunCliAsync(builder);
    }
}
