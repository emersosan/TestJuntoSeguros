using System;
using System.Collections.Generic;
using System.Text;

namespace TestJuntoSeguros.Domain.Model.User
{
  public class UserView
  {
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool LockedOut { get; set; }
  }
}
