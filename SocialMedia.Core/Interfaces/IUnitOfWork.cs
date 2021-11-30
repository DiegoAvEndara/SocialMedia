using SocialMedia.Core.Entities;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    //Cambiamos el IRepository<POst> por PostRepository
    //IRepository<Post> PostRepository { get; }
    IPostRepository PostRepository { get; }
    IRepository<User> UserRepository { get; }
    IRepository<Comment> CommentRepository { get; }
    ISecurityRepository SecurityRepository { get; }
    void SaveChanges();
    Task SaveChangesAsync();
  }
}
