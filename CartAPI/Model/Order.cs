using MongoDB.Bson;

namespace CartAPI.Model
{
    public class Order
    {
        public ObjectId _id { get; set; }
        public string userName { get; set; }
        public string productId { get; set; }
        public int quantity { get; set; }
    }
}
