using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Features.Users.Commands;
using ToDoApi.Application.Features.Users.Queries;
using ToDoApi.Application.QueryParameters;
using ToDoApi.Domain;
using ToDoApi.Domain.Entities;
using ToDoApi.Api.Models;
using Microsoft.Extensions.Options;


namespace ToDoApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AppSettings _appSettings;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator,
            IOptions<AppSettings> appSettings,
            ILogger<UserController> logger)
        {
            _mediator = mediator;
            _appSettings = appSettings.Value;
            _logger = logger;

            _logger.LogInformation("ToDoController initialized. AppName: {AppName}, MaxPageSize: {MaxPageSize}",
                      _appSettings.AppName, _appSettings.MaxPageSize);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll([FromQuery] UserQuaryFilterSortingParameters QuaryParameters)
        {
            _logger.LogInformation("GetAll Users endpoint called with PageNumber: {PageNumber}, PageSize: {PageSize}",
                                   QuaryParameters.PageNumber, QuaryParameters.PageSize);
            // Limit page size based on AppSettings
            if (QuaryParameters.PageSize > _appSettings.MaxPageSize)
            {
                _logger.LogWarning("Requested PageSize {RequestedPageSize} exceeds MaxPageSize {MaxPageSize}. Adjusting to MaxPageSize.",
                                   QuaryParameters.PageSize, _appSettings.MaxPageSize);
                QuaryParameters.PageSize = _appSettings.MaxPageSize;
            }

            // Create the query object
            var query = new GetAllUsersQuery(QuaryParameters);
            // Send the query via MediatR
            var users = await _mediator.Send(query);
            if (!users.Any())
            {
                _logger.LogInformation("No users found.");
                return Ok(new ApiResponse<IEnumerable<User>>(true, "No User Found - list is empty", users));
            }
            _logger.LogInformation("Successfully fetched {UserCount} users.", users.Count());
            return Ok(new ApiResponse<IEnumerable<User>>(true, " All Users fetched successfully", users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {

            _logger.LogInformation("GetById User endpoint called for ID: {UserId}", id);
            // Create the query object
            var query = new GetUserByIdQuery(id);
            // Send the query via MediatR
            var user = await _mediator.Send(query);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                return NotFound(new ApiResponse<User>(false, "User not found", null));
            }
            _logger.LogInformation("User with ID {UserId} found and fetched successfully.", id);
            return Ok(new ApiResponse<User>(true, "User found and fetched successfully", user));
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserDto input)
        {
            _logger.LogInformation("Create User endpoint called for Name: {UserName}", input.Name);
            // Create the command object
            var command = new CreateUserCommand(input);
            // Send the command via MediatR
            var user = await _mediator.Send(command);
            _logger.LogInformation("New User created with ID: {UserId}", user.UserId);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, new ApiResponse<User>(true, "New User created successfully", user));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(int id, [FromBody] UpdateUserDto input)
        {
            _logger.LogInformation("Update User endpoint called for ID: {UserId}", id);
            // Create the command object
            var command = new UpdateUserCommand(id, input);
            // Send the command via MediatR
            var user = await _mediator.Send(command);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for update.", id);
                return NotFound(new ApiResponse<User>(false, "User not found", null));
            }
            _logger.LogInformation("User with ID {UserId} updated successfully.", id);
            return Ok(new ApiResponse<User>(true, "User updated successfully", user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete User endpoint called for ID: {UserId}", id);
            // Create the command object
            var command = new DeleteUserCommand(id);
            // Send the command via MediatR
            var result = await _mediator.Send(command);
            if (!result)
            {
                _logger.LogWarning("User with ID {UserId} not found for deletion.", id);
                return NotFound(new ApiResponse<string>(false, "User not found - please input valid User ID", null));
            }
            _logger.LogInformation("User with ID {UserId} deleted.", id);
            return Ok(new ApiResponse<string>(true, "User deleted successfully", null));
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
