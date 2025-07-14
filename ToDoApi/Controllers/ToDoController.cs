using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;
using ToDoApi.Models.DTOs;
using ToDoApi.Services.Interfaces;

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
            var task = _Service.GetAll();
            if (!task.Any())
            {
                return Ok(new ApiResponse<IEnumerable<ToDo>>(true, "No Task Found - list is empty", task));
            }
            return Ok(new ApiResponse<IEnumerable<ToDo>>(true, " All Tasks fetched successfully", task));
        }

        [HttpGet("{id}")]
        public ActionResult<ToDo> GetById(int id)
        {
            var task = _Service.GetById(id);
            if (task == null) return NotFound(new ApiResponse<ToDo>(false, "Task not found", null));
            return Ok(new ApiResponse<ToDo>(true, "Task found and fetched successfully", task));
        }

        [HttpPost]
        public ActionResult<ToDo> Create([FromBody] CreateToDoDto input)
        {

            var task = _Service.Create(input);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, new ApiResponse<ToDo>(true, "New Task created successfully", task));
        }

        [HttpPut("{id}")]
        public ActionResult<ToDo> Update(int id, [FromBody] UpdateToDoDto input)
        {
            var task = _Service.Update(id, input);
            if (task == null) return NotFound(new ApiResponse<ToDo>(false, "Task not found", null));
            return Ok (new ApiResponse<ToDo>(true, "Task updated successfully", task));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = _Service.Delete(id);
            if (!task) return NotFound(new ApiResponse<string>(false, "Task not found", null));
            return Ok(new ApiResponse<string>(true, "Task deleted successfully", null));
        }

        /*
        [HttpGet("make_error")]
        public IActionResult MakeError()
        {
           
            throw new Exception("This is a test error from MakeError endpoint.");
        }
        */
    }
}