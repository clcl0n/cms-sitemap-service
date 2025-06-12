using System;
using System.Xml;
using System.Xml.Linq;

namespace Cms.SitemapService.Domain.Builders;

public sealed class SitemapIndexBuilder
{
    private readonly XNamespace Xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

    private readonly XDocument Result;

    private readonly XElement XmlRoot;

    public SitemapIndexBuilder()
    {
        XmlRoot = new XElement(Xmlns + "sitemapindex");
        Result = new XDocument(new XDeclaration("1.0", "UTF-8", null), XmlRoot);
    }

    public void AddSitemaps(string sitemapUrlTemplate, long count)
    {
        if (count == 0)
        {
            return;
        }

        for (long i = 1; i <= count; i++)
        {
            var urlElement = CreateSitemapElement(
                string.Format(sitemapUrlTemplate, i),
                DateTime.UtcNow
            );

            XmlRoot.Add(urlElement);
        }
    }

    public string GetResult()
    {
        return Result.ToString();
    }

    private XElement CreateSitemapElement(string loc, DateTime lastModified)
    {
        return new XElement(
            Xmlns + "sitemap",
            new XElement(Xmlns + "loc", loc),
            new XElement(
                Xmlns + "lastmod",
                XmlConvert.ToString(lastModified, XmlDateTimeSerializationMode.Utc)
            )
        );
    }
}
