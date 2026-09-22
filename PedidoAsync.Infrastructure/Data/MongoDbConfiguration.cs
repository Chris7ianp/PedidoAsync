using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace PedidoAsync.Infrastructure.Data;

public static class MongoDbConfiguration
{
    public static void Configure()
    {
        BsonSerializer.RegisterSerializer(
            new GuidSerializer(GuidRepresentation.Standard));
    }
}