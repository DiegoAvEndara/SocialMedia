using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
  public interface IPostRepository : IRepository<Post>
  {
    /* Task<IEnumerable<Post>> GetPosts();*/
    //Debido a la reestructuración de las interfaces, ya no necesitaríamos las siguientes propiedades, o cambiaríamos por:
    //Task<IEnumerable<Post>> GetPosts();
    //Task<Post> GetPost(int id);
    //Task InsertPost(Post post);
    //Task<bool> UpdatePost(Post post);
    //Task<bool> DeletePost(int id);
    Task<IEnumerable<Post>> GetPostsByUser(int userId);

  }
}
