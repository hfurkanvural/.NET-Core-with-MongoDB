using docker_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace docker_api.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class KnowledgeController: Controller
    {

        ILogger<KnowledgeController> _logger;
        // public KnowledgeController(ILogger<KnowledgeController> logger)
        // {
        //     _logger = logger;
        // }
        private readonly IKnowledgeRepository _repo;
        public KnowledgeController(ILogger<KnowledgeController> logger, IKnowledgeRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }
        // GET api/knowledge
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Knowledge>>> Get()
        {
            
            _logger.LogInformation("Get method of knowledge controller executed at {date}", DateTime.UtcNow);
            return new ObjectResult(await _repo.GetAllKnowledgeBase());
        }
        // GET api/knowledge/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Knowledge>> Get(long id)
        {
             _logger.LogInformation("Get method with id of knowledge controller executed at {date}", DateTime.UtcNow);
            var knowledge = await _repo.GetKnowledge(id);
            if (knowledge == null)
                return new NotFoundResult();
            
            return new ObjectResult(knowledge);
        }
        // POST api/knowledge
        [HttpPost]
        public async Task<ActionResult<Knowledge>> Post([FromBody] Knowledge knowledge)
        {
             _logger.LogInformation("Post method of knowledge controller executed at {date}", DateTime.UtcNow);
            knowledge.id = await _repo.GetNextId();
            await _repo.Create(knowledge);
            return new OkObjectResult(knowledge);
        }
        // PUT api/knowledge/1
        [HttpPut("{id}")]
        public async Task<ActionResult<Knowledge>> Put(long id, [FromBody] Knowledge knowledge)
        {
             _logger.LogInformation("Put method of knowledge controller executed at {date}", DateTime.UtcNow);
            
            var knowledgeFromDb = await _repo.GetKnowledge(id);
            try
            {
                if (knowledgeFromDb == null)
                    throw new InvalidKnowledgeException(id);
            }
            catch(InvalidKnowledgeException ex)
            {
                _logger.LogError(ex, "Error Found!");
                return new NotFoundResult();
            }
           
            knowledge.id = knowledgeFromDb.id;
            knowledge.InternalId = knowledgeFromDb.InternalId;
            await _repo.Update(knowledge);
            return new OkObjectResult(knowledge);
        }
        // DELETE api/knowledge/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
             _logger.LogInformation("Delete method of knowledge controller executed at {date}", DateTime.UtcNow);
            var post = await _repo.GetKnowledge(id);
            if (post == null)
                return new NotFoundResult();
            await _repo.Delete(id);
            return new OkResult();
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            Exception newEx = new System.IO.FileNotFoundException();

            _logger.LogError(newEx, newEx.Message);
            // try
            // {
            //     throw new Exception("oops! something went wrong!?");
            // }
            // catch (Exception ex)
            // {
            //     _logger.LogError(ex, "Error Found!");
            // }

            return new BadRequestResult();

        }

    }
}