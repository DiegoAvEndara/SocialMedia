using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infraestructure.Repositories
{

  public class PostMongoRepository : IPostRepository
  {
    private readonly SocialMediaContext _context;
    public PostMongoRepository(SocialMediaContext context)
    {
      _context = context;
    }
    /*public async Task<IEnumerable<Post>> GetPosts()
    {
      var posts = Enumerable.Range(1, 10).Select(x => new Post
      {
        PostId = x,
        Description = $"Description Mongo: {x}",
        Date = DateTime.Now,
        Image = $"https://misapis.com/{x}",
        UserId = x * 2 -1
      });
      await Task.Delay(10);
      return posts;
    }*/
    public async Task<IEnumerable<Publicacion>> GetPosts()
    {
      var posts = await _context.Publicacion.ToListAsync();
      return posts;
    }
  }
}
