using Microsoft.AspNetCore.Mvc;
using ToDoApi.Application.Interfaces.Services; // <-- Remove this
using ToDoApi.Application.QueryParameters;
using ToDoApi.Application.DTOs;
using ToDoApi.Domain;
using ToDoApi.Domain.Entities;
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Features.ToDos.Queries;

namespace ToDoApi.Api.Controllers
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
        public async Task<ActionResult<IEnumerable<ToDo>>> GetAll([FromQuery] ToDoQuaryFilterSortingParameters QuaryParameters)
        {   
            var task = await _Service.GetAllAsync(QuaryParameters);
            if (!task.Any())
            {
                return Ok(new ApiResponse<IEnumerable<ToDo>>(true, "No Task Found - list is empty", task));
            }
            return Ok(new ApiResponse<IEnumerable<ToDo>>(true, " All Tasks fetched successfully", task));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetById(int id)
        {
            var task = await _Service.GetByIdAsync(id);
            if (task == null) return NotFound(new ApiResponse<ToDo>(false, "Task not found", null));
            return Ok(new ApiResponse<ToDo>(true, "Task found and fetched successfully", task));
        }

        [HttpPost]
        public async Task<ActionResult<ToDo>> Create([FromBody] CreateToDoDto input)
        {

            var task = await _Service.CreateAsync(input);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, new ApiResponse<ToDo>(true, "New Task created successfully", task));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDo>> Update(int id, [FromBody] UpdateToDoDto input)
        {
            var task = await _Service.UpdateAsync(id, input);
            if (task == null) return NotFound(new ApiResponse<ToDo>(false, "Task not found", null));
            return Ok (new ApiResponse<ToDo>(true, "Task updated successfully", task));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _Service.DeleteAsync(id);
            if (!task) return NotFound(new ApiResponse<string>(false, "Task not found", null));
            return Ok(new ApiResponse<string>(true, "Task deleted successfully", null));
        }
    }
}