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
        public ActionResult<IEnumerable<User>> GetAll()
        {
            var users = _Service.GetAll();
            if (!users.Any())
            {
                return Ok(new ApiResponse<IEnumerable<User>>(true, "No User Found - list is empty", users));
            }
            return Ok(new ApiResponse<IEnumerable<User>>(true, "All Users fetched successfully", users));
        }
        [HttpGet("{id}")]  
        public ActionResult<User> GetById(int id)
        {
            var user = _Service.GetById(id);
            if (user == null) return NotFound(new ApiResponse<User>(false, "User not found", null));
            return Ok(new ApiResponse<User>(true, "User found and fetched successfully", user));
        }
        [HttpPost]
        public ActionResult<User> Create([FromBody] CreateUserDto input)
        {
            var user = _Service.Create(input);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, new ApiResponse<User>(true, "New User created successfully", user));
        }
        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] UpdateUserDto input)
        {
            var user = _Service.Update(id, input);
            if (user == null) return NotFound(new ApiResponse<User>(false, "User not found", null));
            return Ok(new ApiResponse<User>(true, "User updated successfully", user));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _Service.Delete(id);
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
