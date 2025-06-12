using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cms.Shared.Helpers;
using Cms.SitemapService.Domain.Constants;
using Cms.SitemapService.Domain.Entities;
using Cms.SitemapService.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace Cms.SitemapService.Infrastructure.Services;

internal sealed class DocumentDatabaseService : IDocumentDatabaseService
{
    private readonly FrozenDictionary<Type, string> _entityToCollectionNameMap;

    private readonly IMongoDatabase _database;

    private readonly MongoClient _client;

    public DocumentDatabaseService(IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        var url = MongoUrl.Create(configuration.GetConnectionString("MongoDb"));
        var clientSettings = MongoClientSettings.FromUrl(url);

        clientSettings.ClusterConfigurator = cb =>
            cb.Subscribe(
                new DiagnosticsActivityEventSubscriber(
                    new InstrumentationOptions { CaptureCommandText = true }
                )
            );

        _client = new MongoClient(clientSettings);

        _database = _client.GetDatabase(url.DatabaseName);
        _entityToCollectionNameMap = GetEntityToCollectionNameMap().ToFrozenDictionary();
    }

    public async Task<long> CountAsync<TSitemap>(CancellationToken cancellationToken)
        where TSitemap : Sitemap
    {
        var collection = GetCollection<TSitemap>();

        var filter = Builders<TSitemap>.Filter.Empty;

        return await collection.CountDocumentsAsync(filter, null, cancellationToken);
    }

    public async Task<List<TSitemap>> PaginateAsync<TSitemap>(
        int skip,
        int limit,
        CancellationToken cancellationToken
    )
        where TSitemap : Sitemap
    {
        var collection = GetCollection<TSitemap>();

        var filter = Builders<TSitemap>.Filter.Empty;
        var sort = Builders<TSitemap>.Sort.Descending(x => x.LastModified);
        var options = new FindOptions<TSitemap>
        {
            Skip = skip,
            Limit = limit,
            Sort = sort,
        };
        var cursor = await collection.FindAsync(filter, options, cancellationToken);

        return await cursor.ToListAsync(cancellationToken);
    }

    public async Task InsertAsync<TSitemap>(TSitemap sitemap, CancellationToken cancellationToken)
        where TSitemap : Sitemap
    {
        var collection = GetCollection<TSitemap>();

        await collection.InsertOneAsync(sitemap, null, cancellationToken);
    }

    public async Task InsertEntityUrlAsync<TSitemap>(
        SitemapUrl sitemapUrl,
        CancellationToken cancellationToken
    )
        where TSitemap : Sitemap
    {
        var collection = GetCollection<TSitemap>();

        var filter =
            Builders<TSitemap>.Filter.Eq(x => x.IsLocked, false)
            & Builders<TSitemap>.Filter.SizeLt(x => x.Urls, SitemapLimits.MaxUrlsPerSitemap);

        // Create an update that adds the new URL to the array of URLs
        var update = Builders<TSitemap>
            .Update.AddToSet(x => x.Urls, sitemapUrl)
            .Set(x => x.LastModified, sitemapUrl.LastModified);

        update = update.SetOnInsert(x => x.Id, Guid.NewGuid());
        update = update.SetOnInsert(x => x.CreatedAt, DateTime.UtcNow);
        update = update.SetOnInsert(x => x.IsLocked, false);

        // Execute the update against the document
        var response = await collection.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = true },
            cancellationToken
        );

        await LockSitemapAsync<TSitemap>(cancellationToken);
    }

    public async Task DeleteEntityUrlsAsync<TSitemap>(
        Guid entityId,
        CancellationToken cancellationToken
    )
        where TSitemap : Sitemap
    {
        var collection = GetCollection<TSitemap>();

        // Create a filter that finds any document containing a URL with this entityId
        var filter = Builders<TSitemap>.Filter.ElemMatch(
            x => x.Urls,
            url => url.EntityId == entityId
        );

        // Create an update that removes all matching URLs from the array
        var update = Builders<TSitemap>.Update.PullFilter(
            x => x.Urls,
            url => url.EntityId == entityId
        );

        // Execute the update against all matching documents
        await collection.UpdateManyAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = false },
            cancellationToken
        );
    }

    public async Task SetupCollections()
    {
        var collections = await _database.ListCollectionNames().ToListAsync();

        foreach (var collectionName in _entityToCollectionNameMap.Values)
        {
            await CreateCollectionIfNotExistsAsync(collectionName, collections);
        }
    }

    private async Task CreateCollectionIfNotExistsAsync(
        string collectionName,
        List<string> existingCollections
    )
    {
        if (existingCollections.Contains(collectionName) is false)
        {
            await _database.CreateCollectionAsync(collectionName);
        }
    }

    private async Task LockSitemapAsync<TSitemap>(CancellationToken cancellationToken)
        where TSitemap : Sitemap
    {
        var collection = GetCollection<TSitemap>();

        var filter =
            Builders<TSitemap>.Filter.Eq(x => x.IsLocked, false)
            & Builders<TSitemap>.Filter.Size(x => x.Urls, SitemapLimits.MaxUrlsPerSitemap);

        var update = Builders<TSitemap>.Update.Set(x => x.IsLocked, true);

        await collection.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = false },
            cancellationToken
        );
    }

    private IMongoCollection<TEntity> GetCollection<TEntity>()
        where TEntity : Sitemap
    {
        if (_entityToCollectionNameMap.TryGetValue(typeof(TEntity), out var collectionName))
        {
            return _database.GetCollection<TEntity>(collectionName);
        }

        throw new NotImplementedException(
            $"Missing collection name config for entity: {typeof(TEntity)}"
        );
    }

    private static Dictionary<Type, string> GetEntityToCollectionNameMap()
    {
        var types = new[] { typeof(PostSitemap) };

        return types.ToDictionary(type => type, type => type.NameToLowerInvariant());
    }
}
