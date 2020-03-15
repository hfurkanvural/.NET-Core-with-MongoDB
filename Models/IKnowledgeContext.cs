namespace docker_api.Models
{
    using MongoDB.Driver;
    public interface IKnowledgeContext
    {
        IMongoCollection<Knowledge> KnowledgeBase { get; }
    }
}