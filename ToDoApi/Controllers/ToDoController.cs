using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;
using ToDoApi.Models.DTOs;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private static List<ToDo> toDoList = new List<ToDo>();
        private static int nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<ToDo>> GetAll()
        {
            return Ok(toDoList);
        }

        [HttpGet("{id}")]
        public ActionResult<ToDo> GetById(int id)
        {
            var task = toDoList.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public ActionResult<ToDo> Create([FromBody] ToDoRequest input)
        {

            var task = new ToDo
            {
                Id = nextId++,
                Title = input.Title,
                Description = input.Description,
                Status = false
            };

            toDoList.Add(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public ActionResult<ToDo> Update(int id, [FromBody] ToDoResponse input)
        {

            var task = toDoList.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            task.Title = input.Title;
            task.Description = input.Description;
            task.Status = input.Status;

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = toDoList.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            toDoList.Remove(task);
            return NoContent();
        }
    }
}