using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using music_artist_full_stack.Models;
using music_artist_full_stack.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace music_artist_full_stack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _commentRepository.DeleteComment(id);
            return NoContent();
        }

        [HttpPost]
        public void Post(Comment comment)
        {
            _commentRepository.AddComment(comment);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Comment comment)

        {
            if (id != comment.Id)
            {
                return BadRequest();
            }
            _commentRepository.EditComment(comment);
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var comment = _commentRepository.GetSingleCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }
    }
}
