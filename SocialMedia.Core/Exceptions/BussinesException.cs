using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.Exceptions
{
  public class BussinesException : Exception
  {
    public BussinesException()
    {

    }
    public BussinesException(string message) : base(message)
    {

    }
  }
}
