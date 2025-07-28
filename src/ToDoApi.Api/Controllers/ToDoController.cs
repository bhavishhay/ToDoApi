using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Features.ToDos.Queries;
using ToDoApi.Application.QueryParameters;
using ToDoApi.Domain;
using ToDoApi.Domain.Entities;
using ToDoApi.Api.Models; 
using Microsoft.Extensions.Options;
 


namespace ToDoApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AppSettings _appSettings; 
        private readonly ILogger<ToDoController> _logger; 

        public ToDoController(IMediator mediator,
            IOptions<AppSettings> appSettings,
            ILogger<ToDoController> logger)
        {
            _mediator = mediator;
            _appSettings = appSettings.Value;
            _logger = logger;


            _logger.LogInformation("ToDoController initialized. AppName: {AppName}, MaxPageSize: {MaxPageSize}",
                                  _appSettings.AppName, _appSettings.MaxPageSize);
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetAll([FromQuery] ToDoQuaryFilterSortingParameters QuaryParameters)
        {
            // This line is commented out to avoid throwing an exception for testing purposes.
            //throw new Exception("This is a test error for endpoints.");

            _logger.LogInformation("GetAll ToDos endpoint called with PageNumber: {PageNumber}, PageSize: {PageSize}",
                                   QuaryParameters.PageNumber, QuaryParameters.PageSize);
            // Limit page size based on AppSettings
            if (QuaryParameters.PageSize > _appSettings.MaxPageSize)
            {
                _logger.LogWarning("Requested PageSize {RequestedPageSize} exceeds MaxPageSize {MaxPageSize}. Adjusting to MaxPageSize.",
                                   QuaryParameters.PageSize, _appSettings.MaxPageSize);
                QuaryParameters.PageSize = _appSettings.MaxPageSize;
            }


            // Create the query object
            var query = new GetAllToDosQuery(QuaryParameters);
            // Send the query via MediatR
            var tasks = await _mediator.Send(query);
            if (!tasks.Any())
            {
                _logger.LogInformation("No tasks found.");
                return Ok(new ApiResponse<IEnumerable<ToDo>>(true, "No Task Found - list is empty", tasks));
            }
            _logger.LogInformation("Successfully fetched {TaskCount} tasks.", tasks.Count());
            return Ok(new ApiResponse<IEnumerable<ToDo>>(true, " All Tasks fetched successfully", tasks));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetById(int id)
        {
            _logger.LogInformation("GetById ToDo endpoint called for ID: {ToDoId}", id);
            // Create the query object
            var query = new GetToDoByIdQuery(id);
            // Send the query via MediatR
            var task = await _mediator.Send(query);
            if (task == null)
            {
                _logger.LogWarning("Task with ID {ToDoId} not found.", id);
                return NotFound(new ApiResponse<ToDo>(false, "Task not found", null)); 
            }
            _logger.LogInformation("Task with ID {ToDoId} found.", id);
            return Ok(new ApiResponse<ToDo>(true, "Task found and fetched successfully", task));
        }

        [HttpPost]
        public async Task<ActionResult<ToDo>> Create([FromBody] CreateToDoDto input)
        {
            _logger.LogInformation("Create ToDo endpoint called for Title: {Title}", input.Title);
            // Create the command object
            var command = new CreateToDoCommand(input);
            // Send the command via MediatR
            var task = await _mediator.Send(command);
            _logger.LogInformation("New ToDo created with ID: {ToDoId}", task.Id);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, new ApiResponse<ToDo>(true, "New Task created successfully", task));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDo>> Update(int id, [FromBody] UpdateToDoDto input)
        {
            _logger.LogInformation("Update ToDo endpoint called for ID: {ToDoId}", id);
            // Create the command object
            var command = new UpdateToDoCommand(id, input);
            // Send the command via MediatR
            var task = await _mediator.Send(command);
            if (task == null)
            {
                _logger.LogWarning("Task with ID {ToDoId} not found for update.", id);
                return NotFound(new ApiResponse<ToDo>(false, "Task not found", null));
            }
            _logger.LogInformation("Task with ID {ToDoId} updated.", id);
            return Ok(new ApiResponse<ToDo>(true, "Task updated successfully", task));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete ToDo endpoint called for ID: {ToDoId}", id);
            // Create the command object
            var command = new DeleteToDoCommand(id);
            // Send the command via MediatR
            var result = await _mediator.Send(command);
            if (!result)
            {
                _logger.LogWarning("Task with ID {ToDoId} not found for deletion.", id);
                return NotFound(new ApiResponse<string>(false, "Task not found", null));
            }
            _logger.LogInformation("Task with ID {ToDoId} deleted.", id);
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