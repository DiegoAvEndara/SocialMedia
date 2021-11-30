using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_Media.Api.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerators;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Api.Controllers
{
  //Solo se debe activar el autorize cuando se tenga un usuario registrado, de lo contrario no podrá ingresar nunca: Ej(admin,admin). Ademas se debe especificar que el tipo de AuthorizeAttribute.Roles autorizado sea el correcto o sea "Administrator"
  //nameof es una propiedad de losenumeradores que permite sacar el valor de un enumerador. RoleType es un enumerador creado por nosotros
  [Authorize(Roles = nameof(RoleType.Administrator))]
  [Produces("application/json")]
  [Route("api/[controller]")]
  [ApiController]
  public class SecurityController : Controller
  {
    private readonly ISecurityService _securityService;
    private readonly IMapper _mapper;
    public SecurityController(ISecurityService securityService, IMapper mapper)
    {
      this._securityService = securityService;
      this._mapper = mapper;
    }
    [HttpPost]
    public async Task<IActionResult> Post(SecurityDto securityDto)
    {
      var security = _mapper.Map<Security>(securityDto);
      await _securityService.RegisterUser(security);
      securityDto = _mapper.Map<SecurityDto>(security);
      var response = new ApiResponse<SecurityDto>(securityDto);
      return Ok(response);
    }
  }
}
