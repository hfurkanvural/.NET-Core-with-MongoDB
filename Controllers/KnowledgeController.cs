namespace docker_api.Controllers
{
    using docker_api.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class KnowledgeController: Controller
    {
        private readonly IKnowledgeRepository _repo;
        public KnowledgeController(IKnowledgeRepository repo)
        {
            _repo = repo;
        }
        // GET api/knowledge
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Knowledge>>> Get()
        {
            return new ObjectResult(await _repo.GetAllKnowledgeBase());
        }
        // GET api/knowledge/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Knowledge>> Get(long id)
        {
            var knowledge = await _repo.GetKnowledge(id);
            if (knowledge == null)
                return new NotFoundResult();
            
            return new ObjectResult(knowledge);
        }
        // POST api/knowledge
        [HttpPost]
        public async Task<ActionResult<Knowledge>> Post([FromBody] Knowledge knowledge)
        {
            knowledge.id = await _repo.GetNextId();
            await _repo.Create(knowledge);
            return new OkObjectResult(knowledge);
        }
        // PUT api/knowledge/1
        [HttpPut("{id}")]
        public async Task<ActionResult<Knowledge>> Put(long id, [FromBody] Knowledge knowledge)
        {
            var knowledgeFromDb = await _repo.GetKnowledge(id);
            if (knowledgeFromDb == null)
                return new NotFoundResult();
            knowledge.id = knowledgeFromDb.id;
            knowledge.InternalId = knowledgeFromDb.InternalId;
            await _repo.Update(knowledge);
            return new OkObjectResult(knowledge);
        }
        // DELETE api/knowledge/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var post = await _repo.GetKnowledge(id);
            if (post == null)
                return new NotFoundResult();
            await _repo.Delete(id);
            return new OkResult();
        }
    }
}