using Microsoft.AspNetCore.Authorization;
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
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_postRepository.GetAllPublishedPosts());
        }

        [HttpPost]
        public void Post(Post post)
        {
            _postRepository.AddPost(post);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _postRepository.DeletePost(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Post post)

        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _postRepository.EditPost(post);
            return NoContent();
        }

        [HttpGet("GetPostWithComments/{id}")]
        public IActionResult GetPostByIdWithComments(int id)
        {
            var post = _postRepository.GetPostByIdWithComments(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var post = _postRepository.GetSinglePostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }


        /*   return Ok(post);
         }

         [HttpGet("user/{id}")]
         public IActionResult GetByUserId(int id)
         {
             var post = _postRepository.GetAllPostsByUser(id);
             if (post == null)
             {
                 return NotFound();
             }
             return Ok(post);
         }

       */

    }
}
