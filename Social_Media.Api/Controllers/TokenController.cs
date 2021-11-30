using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
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
    private readonly ISecurityService _securityService;
    public TokenController(IConfiguration configuration, ISecurityService securityService)
    {
      this._configuration = configuration;
      this._securityService = securityService;
    }
    [HttpPost]
    //public IActionResult Authentication(UserLogin login)
    public async Task<IActionResult> Authentication(UserLogin login)
    {
      //if is a valid user

      //if (IsValidUser(login))
      //{
      //  var token = GenerateToken();
      //  return Ok(new { token = token }); // si las variables se llaman igual se puede simplificar con token en un json de tipo c#
      //}
      var validation = await IsValidUser(login);
      if (validation.Item1)
      {
        var token = GenerateToken(validation.Item2);
        return Ok(new { token });
      }
      return NotFound();
      
    }
    //private bool Task<(bool, Security)> IsValidUser (UserLogin login)
    //Necesitamos que nos devuelva 2 cosas deiferentes para eso usamos una tupla que son 2 valores entre paréntesis (,)
    private async Task<(bool, Security)> IsValidUser(UserLogin login)
    {
      //var user = await _securityService.GetLoginByCredentials(login);
      //El método nos devolverá 2 valores
      var user = await _securityService.GetLoginByCredentials(login);
      return (user != null, user);
    }
    //private string GenerateToken(){
    private string GenerateToken(Security security)
    {
      //Header
      var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));// Ésto es lo mismo que usanos en el Startup.cs
      var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
      var header = new JwtHeader(signingCredentials);

      //Claims, caracteriscticas del usuario que queremos añadir, los claims son necesarios para crear el payload
      var claims = new[]
      {
        new Claim(ClaimTypes.Name, security.UserName),
        new Claim("User", security.User),
        new Claim(ClaimTypes.Role, security.Role.ToString())
      };

      //payloads
      var payload = new JwtPayload
      (
        _configuration["Authentication:Issuer"],
        _configuration["Authentication:Audience"],
        claims,
        DateTime.Now,
        DateTime.UtcNow.AddMinutes(10)//Añadimos 10 minutos a la duración del token
      );
      var token = new JwtSecurityToken(header, payload);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
