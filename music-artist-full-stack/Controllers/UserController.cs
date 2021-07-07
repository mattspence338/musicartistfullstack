using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using music_artist_full_stack.Models;
using music_artist_full_stack.Repositories;
using System;


namespace music_artist_full_stack.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{firebaseUserId}")]
        public IActionResult GetUserProfile(string firebaseUserId)
        {
            return Ok(_userRepository.GetByFirebaseUserId(firebaseUserId));
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            user.DateCreated = DateTime.Now;
            user.UserTypeId = 2;
            _userRepository.AddUser(user);
            return CreatedAtAction(
                nameof(GetUserProfile),
                new { firebaseUserId = user.FirebaseUserId },
                user);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, User user)

        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _userRepository.EditUser(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userRepository.DeleteUser(id);
            return NoContent();
        }
    }
}
