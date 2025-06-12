using Cms.Shared.Handlers.Interfaces.Base;
using Cms.SitemapService.Application.Contracts.Queries;

namespace Cms.SitemapService.Application.Handlers.Queries.Interfaces;

public interface IPostGetSitemapIndexQueryHandler : IBaseHandler<PostGetSitemapIndexQuery, string?>;
