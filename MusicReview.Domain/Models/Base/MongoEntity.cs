using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicReview.Domain.Models.Base;

public class MongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id {get; set;}
}