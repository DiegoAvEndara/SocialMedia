﻿using Microsoft.EntityFrameworkCore;
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
    public async Task<IEnumerable<Post>> GetPosts()
    {
      var posts = await _context.Posts.ToListAsync();
      return posts;
    }
    public async Task<Post> GetPost(int id)
    {
      var post = await _context.Posts.FirstOrDefaultAsync(a => a.Id == id);
      return post;
    }
    public async Task InsertPost(Post post)
    {
      _context.Posts.Add(post);
      await _context.SaveChangesAsync();
    }
    public async Task<bool> UpdatePost(Post post)
    {
      var currentPost = await GetPost(post.Id);
      currentPost.Date = post.Date;
      currentPost.Description = post.Description;
      currentPost.Image = post.Image;
      int rows = await _context.SaveChangesAsync();
      return rows > 0;
    }
    public async Task<bool> DeletePost(int id)
    {
      var currentPost = await GetPost(id);
      _context.Posts.Remove(currentPost);
      int rows = await _context.SaveChangesAsync();
      return rows > 0;
    }

    public Task<IEnumerable<Post>> GetPostByUser(int userId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Post> GetAll()
    {
      throw new NotImplementedException();
    }

    public Task<Post> GetById(int id)
    {
      throw new NotImplementedException();
    }

    public Task Add(Post entity)
    {
      throw new NotImplementedException();
    }

    public void Update(Post entity)
    {
      throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<Post>> GetPostsByUser(int userId)
    {
      throw new NotImplementedException();
    }
  }
}
