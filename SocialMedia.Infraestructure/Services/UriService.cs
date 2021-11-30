using SocialMedia.Core.QueryFilter;
using SocialMedia.Infraestructure.Intefaces;
using System;

namespace SocialMedia.Infraestructure.Services
{
  //La interfaz será la que inyectaremos cuando queramos utilizar éstos métodos
  public class UriService : IUriService
  {
    public readonly string _baseUri;
    //El servicio se armará a partir de una URL enviada al constructor
    public UriService(string baseUri)
    {
      _baseUri = baseUri;
    }
    //Se recibrá un string que representa al método al que queremos redireccionar
    public Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl)
    {
      string baseUrl = $"{_baseUri}{actionUrl}";
      return new Uri(baseUrl);
    }
  }
}
