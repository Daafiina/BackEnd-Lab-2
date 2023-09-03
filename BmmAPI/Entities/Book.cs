using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BmmAPI.Entities
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string BookName { get; set; } = "";
        public string Author { get; set; } = "";
        public DateTime PublishedDate { get; set; }
        public string? BookGenre { get; set; }

    }
}
