using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogProjectAPI.DAL.Abstract;
using BlogProjectAPI.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BlogProjectAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IPostsRepository _postsRepository;
        public PostsController(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        /// <summary>
        /// İstenen kadar Post'u çekmek için kullanılır.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        
        [HttpPost]
        [Route("GetPosts")]
        public async Task<IActionResult> GetPosts([FromBody]GetPostsModel model)
        {
            var result = await _postsRepository.GetAll(model);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}