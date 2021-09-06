﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialMedia.Core.DTOs
{
  public class PostDto
  {
    public int PostId { get; set; }
    public int UserId { get; set; }
    public DateTime? Date { get; set; }
    //Añadimos requerido para contrastar con el controlador
    [Required]
    public string Description { get; set; }
    public string Image { get; set; }
  }
}
