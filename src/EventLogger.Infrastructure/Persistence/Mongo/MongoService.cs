using EventLogger.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventLogger.Infrastructure.Persistence.Mongo;

public class MongoService : IMongoService
{
    private readonly IMongoCollection<BsonDocument> _collection;

    public MongoService(IMongoClient client, IConfiguration configuration)
    {
        var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
        _collection = database.GetCollection<BsonDocument>("EventDetails");
    }

    public async Task SaveEventDetailsAsync(Guid eventId, string detailsJson)
    {
        var doc = BsonDocument.Parse(detailsJson);
        doc["_id"] = eventId.ToString();
        await _collection.ReplaceOneAsync(
            filter: Builders<BsonDocument>.Filter.Eq("_id", eventId.ToString()),
            options: new ReplaceOptions { IsUpsert = true },
            replacement: doc);
    }

    public async Task<string?> GetEventDetailsAsync(Guid eventId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", eventId.ToString());
        var doc = await _collection.Find(filter).FirstOrDefaultAsync();
        return doc?.ToJson();
    }
}
