using backend.Filters;
using backend.Models.Entityes.UserEntityes;
using backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("/getUser/{login}")]
        public async Task<IResult> GetUser(string login)
        {
            return await _userService.GetUser(login);
        }

        [HttpGet("/getUserById/{id}")]
        public async Task<IResult> GetUserById(int id)
        {
            return await _userService.GetUserById(id);
        }

        [HttpGet("/getUsers")]
        public async Task<IResult> GetUsers()
        {
            return await _userService.GetUsers();
        }

        [HttpPost("/createUser")]
        public async Task<IResult> UserRegistration([FromBody] UserEntity userData)
        {
            return await _userService.CreateUser(userData);
        }


        [HttpPut("updateUser")]
        [ServiceFilter(typeof(UserAccessControllerFilter))]
        public async Task<IResult> UpdateUser([FromBody] UserEntity userData, [FromQuery] string login)
        {
            return await _userService.UpdateUser(login, userData);
        }

        [HttpDelete("/deleteUser")]
        [ServiceFilter(typeof(UserAccessControllerFilter))]
        public async Task<IResult> DeleteUser([FromQuery] string login)
        {
            return await _userService.DeleteUser(login);
        }
    }
}
