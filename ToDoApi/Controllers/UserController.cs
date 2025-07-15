using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;
using ToDoApi.Models.DTOs;
using ToDoApi.Services.Interfaces;

namespace ToDoApi.Controllers
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
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _Service.GetAllAsync();
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
        //[HttpDelete]
        //public IActionResult DeleteAll()
        //{
        //    var users = _Service.GetAll();
        //    if (!users.Any()) return NotFound(new ApiResponse<string>(false, "No Users found - list is empty", null));
        //    foreach (var user in users)
        //    {
        //        _Service.Delete(user.UserId);
        //    }
        //    return Ok(new ApiResponse<string>(true, "All Users deleted successfully", null));
        //}
    }
}
