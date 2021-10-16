using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Social_Media.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TokenController : ControllerBase
  {
    private readonly IConfiguration _configuration;
    public TokenController(IConfiguration configuration)
    {
      this._configuration = configuration;
    }
    [HttpPost]
    public IActionResult Authentication(UserLogin login)
    {
      //if is a valid user
      if (IsValidUser(login))
      {
        var token = GenerateToken();
        return Ok(new { token = token }); // si las variables se llaman igual se puede simplificar con token en un json de tipo c#
      }
      return NotFound();
    }
    private bool IsValidUser(UserLogin login)
    {
      return true;
    }

    private string GenerateToken(){
      //Header
      var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));// Ésto es lo mismo que usanos en el Startup.cs
      var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
      var header = new JwtHeader(signingCredentials);

      //Claims, caracteriscticas del usuario que queremos añadir, los claims son necesarios para crear el payload
      var claims = new[]
      {
        new Claim(ClaimTypes.Name, "Diego Avendano"),
        new Claim(ClaimTypes.Email, "diego@gmail.com"),
        new Claim(ClaimTypes.Role, "Administrador"),
      };

      //payloads
      var payload = new JwtPayload
      (
        _configuration["Authentication:Issuer"],
        _configuration["Authentication:Audience"],
        claims,
        DateTime.Now,
        DateTime.UtcNow.AddMinutes(3)
      );
      var token = new JwtSecurityToken(header, payload);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
