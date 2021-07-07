using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using music_artist_full_stack.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace music_artist_full_stack.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostTypeController : ControllerBase
    {
        private readonly IPostTypeRepository _postTypeRepository;
        public PostTypeController(IPostTypeRepository postTypeRepository)
        {
            _postTypeRepository = postTypeRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_postTypeRepository.GetAllPostTypes());
        }
    }
}
