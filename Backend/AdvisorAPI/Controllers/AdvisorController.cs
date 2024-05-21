using Microsoft.AspNetCore.Mvc;
using AdvisorAPI.Models;
using AdvisorAPI.Repositories;
using System.Linq;

namespace AdvisorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvisorController(IAdvisorRepository repository) : ControllerBase
    {
        private readonly IAdvisorRepository _repository = repository;

        [HttpPost]
        public ActionResult<Advisor> CreateAdvisor([FromBody] Advisor advisor)
        {
            advisor.HealthStatus = GenerateRandomHealthStatus();
            var createdAdvisor = _repository.Create(advisor);
            return CreatedAtAction(nameof(GetAdvisor), new { id = createdAdvisor.Id }, createdAdvisor);
        }

        [HttpGet("{id}")]
        public ActionResult<Advisor> GetAdvisor(int id)
        {
            var advisor = _repository.Get(id);
            if (advisor == null)
            {
                return NotFound();
            }
            return Ok(advisor);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAdvisor(int id, [FromBody] Advisor advisor)
        {
            if (id != advisor.Id)
            {
                return BadRequest();
            }

            var existingAdvisor = _repository.Get(id);
            if (existingAdvisor == null)
            {
                return NotFound();
            }

            _repository.Update(advisor);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAdvisor(int id)
        {
            var advisor = _repository.Get(id);
            if (advisor == null)
            {
                return NotFound();
            }

            _repository.Delete(id);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Advisor>> ListAdvisors()
        {
            var advisors = _repository.List();
            return Ok(advisors);
        }

        private string GenerateRandomHealthStatus()
        {
            var rnd = new Random();
            int value = rnd.Next(100);
            if (value < 60) return "Green";
            if (value < 80) return "Yellow";
            return "Red";
        }
    }
}
