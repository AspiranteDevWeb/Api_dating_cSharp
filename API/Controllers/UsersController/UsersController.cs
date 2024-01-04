using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.UsersController
{
    [Authorize]
    public class UsersController: BaseApiController
    {

        private readonly IUserRepository _userRepository;
        public UsersController (IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //[AllowAnonymous]
        [HttpGet]

        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users) ;
        }

      //  [Authorize]
        [HttpGet("{username}")]

        public async Task<ActionResult<AppUser>> GetUser(string username)
        {
            return Ok(await _userRepository.GetUserByUsernameAsync(username));
   
        }
    }
}