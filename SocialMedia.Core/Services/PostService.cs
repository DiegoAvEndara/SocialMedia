using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
  public class PostService : IPostService
  {
    /*Se cambiará toda la lógica para  que coninsida con el repositorio y la interfaz genérica
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    public PostService(IPostRepository postRepository, IUserRepository userRepository)
    {
      _postRepository = postRepository;
      _userRepository = userRepository;
    }
    */
    /*Se remplaza el patrón Generic Repository por el de Unit of Work
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<User> _userRepository;*/
    private readonly IUnitOfWork _unitOfWork;
    /* Cambiamos la lógica para solo recibir unitOfWork en lugar de IRepository
    public PostService(IRepository<Post> postRepository, IRepository<User> userRepository)
    {
      _postRepository = postRepository;
      _userRepository = userRepository;
    }*/
    public PostService(IUnitOfWork unitOfWork)
    {
      this._unitOfWork = unitOfWork;
    }
    public async Task<bool> DeletePost(int id)
    {
      /*Se cambia por la implenmentación genérica que ya no contiene DeletePost(id)
      return await _postRepository.DeletePost(id);*/
      await _unitOfWork.PostRepository.Delete(id);
      return true;
    }

    public async Task<Post> GetPost(int id)
    {
      return await _unitOfWork.PostRepository.GetById(id);
      
    }

    public async Task<IEnumerable<Post>> GetPosts()
    {
      return await _unitOfWork.PostRepository.GetAll();
    }

    public async Task InsertPost(Post post)
    {
      var user = await _unitOfWork.UserRepository.GetById(post.UserId);
      if (user == null)
      {
        throw new Exception("User doesn't exist");
      }
      if (post.Description.Contains("Sexo"))
      {
        throw new Exception("Contenido no permitido");
      }
      await _unitOfWork.PostRepository.Add(post);
    }

    public async Task<bool> UpdatePost(Post post)
    {
      await _unitOfWork.PostRepository.Update(post);
      return true;
    }
  }
}
