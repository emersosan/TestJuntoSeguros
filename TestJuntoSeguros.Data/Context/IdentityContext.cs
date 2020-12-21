using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TestJuntoSeguros.Data.Context
{
  public class IdentityContext : IdentityDbContext<IdentityUser, IdentityRole, string>
  {
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }
  }
}
