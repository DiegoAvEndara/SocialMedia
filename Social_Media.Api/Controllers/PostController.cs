using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Social_Media.Api.Responses;
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
    public readonly IPostService _postService;
    private readonly IMapper _mapper;
    public PostController(IPostService postService, IMapper mapper)
    {
      this._postService = postService;
      this._mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
      var posts = await _postService.GetPosts();
      /*var postsDto = posts.Select(x => new PostDto
      {
        PostId = x.PostId,
        Date = x.Date,
        Description = x.Description,
        Image = x.Image,
        UserId = x.UserId
      });*/
      var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
      var response = new ApiResponse<IEnumerable<PostDto>>(postsDto);
      return Ok(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
      var post = await _postService.GetPost(id);
      /*var postDto = new PostDto
      {
        PostId = post.PostId,
        Date = post.Date,
        Description = post.Description,
        Image = post.Image,
        UserId = post.UserId
      };*/
      var postDto = _mapper.Map<PostDto>(post);
      var response = new ApiResponse<PostDto>(postDto);
      return Ok(response);

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
      await _postService.InsertPost(newPost);
      postDto = _mapper.Map<PostDto>(newPost);
      var response = new ApiResponse<PostDto>(postDto);
      return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put (int id, PostDto postDto)
    {
      var post = _mapper.Map<Post>(postDto);
      post.Id = id;
      var result = await _postService.UpdatePost(post);
      var response = new ApiResponse<bool>(result);
      return Ok(response);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _postService.DeletePost(id);
      var response = new ApiResponse<bool>(result);
      return Ok(response);
    }
  }
}
