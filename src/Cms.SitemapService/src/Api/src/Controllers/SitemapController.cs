using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Cms.SitemapService.Application.Contracts.Queries;
using Cms.SitemapService.Application.Handlers.Queries.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cms.SitemapService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SitemapController(
    IPostGetSitemapIndexQueryHandler postGetSitemapIndexQueryHandler,
    IPostGetSitemapQueryHandler postGetSitemapQueryHandler
) : ControllerBase
{
    [HttpGet("post/sitemap-index.xml")]
    [Produces(MediaTypeNames.Application.Xml)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPostIndexSitemap(CancellationToken cancellationToken)
    {
        var response = await postGetSitemapIndexQueryHandler.HandleAsync(
            new PostGetSitemapIndexQuery(GetPostSitemapUrlTemplate()),
            cancellationToken
        );

        return response is not null
            ? Content(response, MediaTypeNames.Application.Xml)
            : NotFound();
    }

    [HttpGet("post/sitemap-{sitemapNumber}.xml")]
    [Produces(MediaTypeNames.Application.Xml)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPostSitemap(
        int sitemapNumber,
        CancellationToken cancellationToken
    )
    {
        var response = await postGetSitemapQueryHandler.HandleAsync(
            new PostGetSitemapQuery(sitemapNumber),
            cancellationToken
        );

        return response is not null
            ? Content(response, MediaTypeNames.Application.Xml)
            : NotFound();
    }

    private string GetPostSitemapUrlTemplate()
    {
        var sampleUrl =
            Url.Action(
                action: nameof(GetPostSitemap),
                controller: "Sitemap",
                values: new { sitemapNumber = 1 },
                protocol: Request.Scheme,
                host: Request.Host.Value
            ) ?? throw new InvalidOperationException("Url.Action returned null");

        return sampleUrl.Replace("1", "{0}");
    }
}
