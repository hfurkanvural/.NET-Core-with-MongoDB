namespace docker_api.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    public class Knowledge
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string details {get; set;}
        public long id { get; set; }
    }
}