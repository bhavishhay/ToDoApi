using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;
using ToDoApi.Models.DTOs;
using ToDoApi.Services;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _Service;

        public ToDoController(IToDoService service)
        {
            _Service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ToDo>> GetAll()
        {
            return Ok(_Service.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<ToDo> GetById(int id)
        {
            var task = _Service.GetById(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public ActionResult<ToDo> Create([FromBody] CreateToDoDto input)
        {

            var task = _Service.Create(input);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public ActionResult<ToDo> Update(int id, [FromBody] UpdateToDoDto input)
        {
            var task = _Service.Update(id, input);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = _Service.Delete(id);
            if (!task) return NotFound();
            return NoContent();
        }
    }
}