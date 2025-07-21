using Microsoft.AspNetCore.Mvc;
using ToDoApi.Application.Interfaces.Services;
using ToDoApi.Application.QueryParameters;
using ToDoApi.Application.DTOs;
using ToDoApi.Domain;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _Service;

        public UserController(IUserService service)
        {
            _Service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll([FromQuery] UserQuaryFilterSortingParameters QuaryParameters)
        {
            var users = await _Service.GetAllAsync(QuaryParameters);
            if (!users.Any())
            {
                return Ok(new ApiResponse<IEnumerable<User>>(true, "No User Found - list is empty", users));
            }
            return Ok(new ApiResponse<IEnumerable<User>>(true, " All Users fetched successfully", users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _Service.GetByIdAsync(id);
            if (user == null) return NotFound(new ApiResponse<User>(false, "User not found", null));
            return Ok(new ApiResponse<User>(true, "User found and fetched successfully", user));
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserDto input)
        {

            var user = await _Service.CreateAsync(input);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, new ApiResponse<User>(true, "New User created successfully", user));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(int id, [FromBody] UpdateUserDto input)
        {
            var user = await _Service.UpdateAsync(id, input);
            if (user == null) return NotFound(new ApiResponse<User>(false, "User not found", null));
            return Ok(new ApiResponse<User>(true, "User updated successfully", user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _Service.DeleteAsync(id);
            if (!result) return NotFound(new ApiResponse<string>(false, "User not found - please input valid User ID", null));
            return Ok(new ApiResponse<string>(true, "User deleted successfully", null));
        }
    }
}
