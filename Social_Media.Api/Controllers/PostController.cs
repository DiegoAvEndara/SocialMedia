using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
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
    private readonly IMapper _mapper;
    public PostController(IPostRepository postRepository, IMapper mapper)
    {
      this._postRepository = postRepository;
      this._mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
      var posts = await _postRepository.GetPosts();
      /*var postsDto = posts.Select(x => new PostDto
      {
        PostId = x.PostId,
        Date = x.Date,
        Description = x.Description,
        Image = x.Image,
        UserId = x.UserId
      });*/
      var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
      return Ok(postsDto);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPosts(int id)
    {
      var post = await _postRepository.GetPost(id);
      /*var postDto = new PostDto
      {
        PostId = post.PostId,
        Date = post.Date,
        Description = post.Description,
        Image = post.Image,
        UserId = post.UserId
      };*/
      var postDto = _mapper.Map<IEnumerable<PostDto>>(post);
      return Ok(postDto);

    }
    [HttpPost]
    public async Task<IActionResult> Post(PostDto postDto)
    {
      //Creamos un nuevo objeto que sea de la entidad post pero para llenarlo usamos la DtoPost, recibida al inicio
      /*var newPost = new Post
      {
        Date = postDto.Date,
        Description = postDto.Description,
        Image = postDto.Image,
        UserId = postDto.UserId
      };*/

      //Revisamos las funcionalidades del decorador ApiController
      /*if (!ModelState.IsValid)
      {
        return BadRequest();
      }*/
      var newPost = _mapper.Map<Post>(postDto);
      //Cambiamos Post por PostDto para cambiar la clase a la que hace referencia
      await _postRepository.InsertPost(newPost);
      return Ok(newPost);
    }
  }
}
