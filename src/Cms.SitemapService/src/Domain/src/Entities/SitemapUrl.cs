using System;
using Cms.SitemapService.Domain.Constants;

namespace Cms.SitemapService.Domain.Entities;

public class SitemapUrl
{
    public required Guid EntityId { get; set; }

    public required string Location { get; set; }

    public required DateTime LastModified { get; set; }

    public required ChangeFrequency ChangeFrequency { get; set; }

    public required double Priority { get; set; }
}
