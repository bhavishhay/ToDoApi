using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Features.Users.Commands;
using ToDoApi.Application.Features.Users.Queries;
using ToDoApi.Application.QueryParameters;
using ToDoApi.Domain;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll([FromQuery] UserQuaryFilterSortingParameters QuaryParameters)
        {
            // Create the query object
            var query = new GetAllUsersQuery(QuaryParameters);
            // Send the query via MediatR
            var users = await _mediator.Send(query);
            if (!users.Any())
            {
                return Ok(new ApiResponse<IEnumerable<User>>(true, "No User Found - list is empty", users));
            }
            return Ok(new ApiResponse<IEnumerable<User>>(true, " All Users fetched successfully", users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            // Create the query object
            var query = new GetUserByIdQuery(id);
            // Send the query via MediatR
            var user = await _mediator.Send(query);
            if (user == null) return NotFound(new ApiResponse<User>(false, "User not found", null));
            return Ok(new ApiResponse<User>(true, "User found and fetched successfully", user));
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserDto input)
        {

            // Create the command object
            var command = new CreateUserCommand(input);
            // Send the command via MediatR
            var user = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, new ApiResponse<User>(true, "New User created successfully", user));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(int id, [FromBody] UpdateUserDto input)
        {
            // Create the command object
            var command = new UpdateUserCommand(id, input);
            // Send the command via MediatR
            var user = await _mediator.Send(command);
            if (user == null) return NotFound(new ApiResponse<User>(false, "User not found", null));
            return Ok(new ApiResponse<User>(true, "User updated successfully", user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Create the command object
            var command = new DeleteUserCommand(id);
            // Send the command via MediatR
            var result = await _mediator.Send(command);
            if (!result) return NotFound(new ApiResponse<string>(false, "User not found - please input valid User ID", null));
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
