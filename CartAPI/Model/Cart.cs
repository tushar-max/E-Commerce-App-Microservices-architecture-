using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CartAPI.Model
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string userName { get; set; }
        public string productId { get; set; }
        public int quantity { get; set; }

    }
}
