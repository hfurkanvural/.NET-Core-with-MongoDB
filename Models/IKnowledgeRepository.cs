namespace docker_api.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IKnowledgeRepository
    {
        // api/[GET]
        Task<IEnumerable<Knowledge>> GetAllKnowledgeBase();
        // api/1/[GET]
        Task<Knowledge> GetKnowledge(long id);
        // api/[POST]
        Task Create(Knowledge knowledge);
        // api/[PUT]
        Task<bool> Update(Knowledge knowledge);
        // api/1/[DELETE]
        Task<bool> Delete(long id);
        Task<long> GetNextId();
    }
}
