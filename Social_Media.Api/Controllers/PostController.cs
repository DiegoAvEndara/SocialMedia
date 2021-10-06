using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Social_Media.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilter;
using SocialMedia.Infraestructure.Intefaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Social_Media.Api.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  [ApiController]
  public class PostController : ControllerBase
  {
    public readonly IPostService _postService;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;
    public PostController(IPostService postService, IMapper mapper, IUriService uriService)
    {
      this._postService = postService;
      this._mapper = mapper;
      this._uriService = uriService;
    }
    //Segun los criterios de filtrado, se debe poder hacerlo a parir del usuario, fecha o descripción
    //Deberíamos recibir los filtros int? userId, DateTime? date, string description pero en su lugar recibiremos un objeto
    //Al momento de mapear los objetos lo normal es definirlos pero como usaremos los mismos nombres no es necesario
    //El FromQuery indica que al momento de recibir los parámetros lo hará por la URL(FromQUery)
    /// <summary>
    /// Retrieve all posts
    /// </summary>
    /// <param name="filters">Filters to apply</param>
    /// <returns></returns>
    [HttpGet (Name = nameof(GetPosts))]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public IActionResult GetPosts([FromQuery]PostQueryFilter filters)
    {
      //Según el patrón unitOfWork posts ya no sería asíncrono ya que la clase PostServices se encarga de eso
      var posts =  _postService.GetPosts(filters);
      //Una vez quitamos el await en post, vemos que no es neceario que la clase tenga async Task y lo quitalmos
      /*var postsDto = posts.Select(x => new PostDto
      {
        PostId = x.PostId,
        Date = x.Date,
        Description = x.Description,
        Image = x.Image,
        UserId = x.UserId
      });*/
      var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
      //Construimos los metadatos que enviaremos por el header
      var metadata = new Metadata
      {
        TotalCount = posts.TotalCount,
        PageSize = posts.PageSize,
        CurrentPage = posts.CurrentPage,
        TotalPages = posts.TotalPages,
        HasNextPage = posts.HasNextPage,
        HasPreviouPage = posts.HasPreviousPage,
        //Se cambia el api/Post por el método Url.RouteUrl() que resuelve ésta parate de la url
        //NextPageUrl = _uriService.GetPostPaginationUri(filters, "/api/Post").ToString()
        NextPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString(),
        PreviousPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString()
      };
      var response = new ApiResponse<IEnumerable<PostDto>>(postsDto)
      {
        Meta = metadata
      };
      //Serialize devuelve un texto en formato json
      Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
      return Ok(response);
    }
    [HttpGet("{id}")]
    //[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
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
