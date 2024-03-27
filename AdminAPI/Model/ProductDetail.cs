using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdminAPI.Model
{
    public class ProductDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }


        //public ObjectId _id {  get; set; }
        public string Product_Name { get; set; }
        public int Price { get; set; }
        public string Size { get; set; }
        public string Design { get; set; }
    }
}
