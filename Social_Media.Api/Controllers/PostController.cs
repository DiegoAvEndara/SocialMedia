using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PostController : ControllerBase
  {
    public readonly IPostRepository _postRepository;
    public PostController(IPostRepository postRepository)
    {
      _postRepository = postRepository;
    }
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
      var posts = await _postRepository.GetPosts();
      return Ok(posts);
    }
  }
}
