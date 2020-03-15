namespace docker_api.Models
{
    using docker_api;
    using MongoDB.Driver;
    using System;
    public class KnowledgeContext: IKnowledgeContext
        {
            private readonly IMongoDatabase _db;
            public KnowledgeContext(MongoDBConfig config)
            {
                var client = new MongoClient(config.ConnectionString);
                _db = client.GetDatabase(config.Database);
            }
            public IMongoCollection<Knowledge> KnowledgeBase => _db.GetCollection<Knowledge>("KnowledgeBase");
        }
}