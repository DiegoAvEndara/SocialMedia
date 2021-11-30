using SocialMedia.Core.QueryFilter;
using System;

namespace SocialMedia.Infraestructure.Intefaces
{
  public interface IUriService
  {
    Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl);
  }
}