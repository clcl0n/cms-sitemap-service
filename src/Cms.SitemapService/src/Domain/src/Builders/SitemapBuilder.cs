using System;
using System.Xml;
using System.Xml.Linq;
using Cms.SitemapService.Domain.Constants;
using Cms.SitemapService.Domain.Entities;

namespace Cms.SitemapService.Domain.Builders;

public sealed class SitemapBuilder
{
    private readonly XNamespace Xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

    private readonly XDocument Result;

    private readonly XElement XmlRoot;

    public SitemapBuilder()
    {
        XmlRoot = new XElement(Xmlns + "urlset");
        Result = new XDocument(new XDeclaration("1.0", "UTF-8", null), XmlRoot);
    }

    public void AddUrls(Sitemap sitemap)
    {
        foreach (var url in sitemap.Urls)
        {
            var urlElement = CreateUrlElement(
                url.Location,
                url.LastModified,
                url.Priority,
                url.ChangeFrequency
            );

            XmlRoot.Add(urlElement);
        }
    }

    public string GetResult()
    {
        return Result.ToString();
    }

    private XElement CreateUrlElement(
        string loc,
        DateTime lastModified,
        double priority,
        ChangeFrequency changeFrequency
    )
    {
        return new XElement(
            Xmlns + "url",
            new XElement(Xmlns + "loc", loc),
            new XElement(
                Xmlns + "lastmod",
                XmlConvert.ToString(lastModified, XmlDateTimeSerializationMode.Utc)
            ),
            new XElement(Xmlns + "priority", priority),
            new XElement(Xmlns + "changefreq", Enum.GetName(changeFrequency))
        );
    }
}
