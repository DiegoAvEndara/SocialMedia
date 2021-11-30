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
  public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
  {
    public SecurityRepository(SocialMediaContext context) : base(context) { }
    public async Task<Security> GetLoginByCredential(UserLogin login)
    {
      return await _entities.FirstOrDefaultAsync(x => x.User == login.User && x.Password == login.Password);
    }
  }
}
