using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialMedia.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SocialMedia.Infraestructure.Filters
{
  public class GlobalExceptionFilter : IExceptionFilter
  {
    public void OnException(ExceptionContext context)
    {
      if (context.Exception.GetType() == typeof(BussinesException))
      {
        //capturamos la exception y lo convertimos a un bussinesException
        var exception = (BussinesException)context.Exception;
        
        //Creamos un nuevo objeto Json con valores guardados en memoria, el estandar para la apiControles usa Detail y los tre puntos siguientes en su contructor json
        var validation = new
        {
          Status = 400,
          Title = "Bad Request",
          Detail = exception.Message
        };
        var json = new
        {
          errors = new[] { validation }
        };
        context.Result = new BadRequestObjectResult(json);
        // se convertirá el badRequst en un status code con un número entero 400
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //Para indicar que una ecepción ser;a controlada ponemos ExceptionHandled = true
        context.ExceptionHandled = true;
      }
      //Luego de realizado éste filtro hay que registrarlo en el middleware que se encuentra en  Startup.cs
    }
  }
}
