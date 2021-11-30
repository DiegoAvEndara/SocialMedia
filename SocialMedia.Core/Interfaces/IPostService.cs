using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
  public interface IPostService
  {
    //MOdificaremos ésta clase para que coincida con el patrón Unit Of Work 
    //Task<IEnumerable<Post>> GetPosts();
    //IEnumerable<Post> GetPosts(PostQueryFilter filters);
    PaginationList<Post> GetPosts(PostQueryFilter filters);
    Task<Post> GetPost(int id);
    Task InsertPost(Post post);
    Task<bool> UpdatePost(Post post);
    Task<bool> DeletePost(int id);
  }
}