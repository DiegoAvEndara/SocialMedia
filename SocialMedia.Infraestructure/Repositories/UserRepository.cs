using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infraestructure.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly SocialMediaContext _context;
    public UserRepository(SocialMediaContext context)
    {
      _context = context;
    }
    public async Task<IEnumerable<User>> GetUsers()
    {
      var posts = await _context.Users.ToListAsync();
      return posts;
    }
    public async Task<User> GetUser(int id)
    {
      var post = await _context.Users.FirstOrDefaultAsync(a => a.UserId == id);
      return post;
    }
  }
}
