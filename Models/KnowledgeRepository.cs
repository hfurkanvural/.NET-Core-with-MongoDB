namespace docker_api.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using MongoDB.Bson;
    using System.Linq;
    public class KnowledgeRepository : IKnowledgeRepository
    {
        private readonly IKnowledgeContext _context;
        public KnowledgeRepository(IKnowledgeContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Knowledge>> GetAllKnowledgeBase()
        {
            return await _context
                            .KnowledgeBase
                            .Find(_ => true)
                            .ToListAsync();
        }
        public Task<Knowledge> GetKnowledge(long id)
        {
            FilterDefinition<Knowledge> filter = Builders<Knowledge>.Filter.Eq(m => m.id, id);
            return _context
                    .KnowledgeBase
                    .Find(filter)
                    .FirstOrDefaultAsync();
        }
        public async Task Create(Knowledge knowledge)
        {
            await _context.KnowledgeBase.InsertOneAsync(knowledge);
        }
        public async Task<bool> Update(Knowledge knowledge)
        {
            ReplaceOneResult updateResult =
                await _context
                        .KnowledgeBase
                        .ReplaceOneAsync(
                            filter: g => g.id == knowledge.id,
                            replacement: knowledge);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> Delete(long id)
        {
            FilterDefinition<Knowledge> filter = Builders<Knowledge>.Filter.Eq(m => m.id, id);
            DeleteResult deleteResult = await _context
                                                .KnowledgeBase
                                              .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
        public async Task<long> GetNextId()
        {
            return await _context.KnowledgeBase.CountDocumentsAsync(new BsonDocument()) + 1;
        }
    }
}