using System;
using System.Collections.Generic;

namespace Cms.SitemapService.Domain.Entities;

public abstract class Sitemap
{
    public required Guid Id { get; set; }

    public required bool IsLocked { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required List<SitemapUrl> Urls { get; set; } = [];

    public required DateTime LastModified { get; set; }
}
