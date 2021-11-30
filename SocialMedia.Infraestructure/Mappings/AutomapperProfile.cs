using AutoMapper;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infraestructure.Mappings
{
  public class AutomapperProfile : Profile
  {
    public AutomapperProfile()
    {
      CreateMap<Post, PostDto>();
      CreateMap<PostDto, Post>();
      CreateMap<Security, SecurityDto>().ReverseMap();//Hace lo mismo pero también en sentido contrario
    }
  }
}
