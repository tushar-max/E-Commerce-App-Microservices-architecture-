using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace InventoryAPI.Model
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Product_Name { get; set; }
        public int Price { get; set; }
        public string Size { get; set; }
        public string Design { get; set; }
    }
}
