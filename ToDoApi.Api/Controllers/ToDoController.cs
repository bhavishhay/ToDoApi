using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Features.ToDos.Queries;
//using ToDoApi.Application.Interfaces.Services; 
using ToDoApi.Application.QueryParameters;
using ToDoApi.Domain;
using ToDoApi.Domain.Entities;


namespace ToDoApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ToDoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetAll([FromQuery] ToDoQuaryFilterSortingParameters QuaryParameters)
        {
            // Create the query object
            var query = new GetAllToDosQuery(QuaryParameters);
            // Send the query via MediatR
            var tasks = await _mediator.Send(query);
            if (!tasks.Any())
            {
                return Ok(new ApiResponse<IEnumerable<ToDo>>(true, "No Task Found - list is empty", tasks));
            }
            return Ok(new ApiResponse<IEnumerable<ToDo>>(true, " All Tasks fetched successfully", tasks));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetById(int id)
        {
            // Create the query object
            var query = new GetToDoByIdQuery(id);
            // Send the query via MediatR
            var task = await _mediator.Send(query);
            if (task == null) return NotFound(new ApiResponse<ToDo>(false, "Task not found", null));
            return Ok(new ApiResponse<ToDo>(true, "Task found and fetched successfully", task));
        }

        [HttpPost]
        public async Task<ActionResult<ToDo>> Create([FromBody] CreateToDoDto input)
        {

            // Create the command object
            var command = new CreateToDoCommand(input);
            // Send the command via MediatR
            var task = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, new ApiResponse<ToDo>(true, "New Task created successfully", task));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDo>> Update(int id, [FromBody] UpdateToDoDto input)
        {
            // Create the command object
            var command = new UpdateToDoCommand(id, input);
            // Send the command via MediatR
            var task = await _mediator.Send(command);
            if (task == null) return NotFound(new ApiResponse<ToDo>(false, "Task not found", null));
            return Ok(new ApiResponse<ToDo>(true, "Task updated successfully", task));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Create the command object
            var command = new DeleteToDoCommand(id);
            // Send the command via MediatR
            var result = await _mediator.Send(command);
            if (!result) return NotFound(new ApiResponse<string>(false, "Task not found", null));
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