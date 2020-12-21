using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetDevPack.Identity;
using NetDevPack.Identity.Jwt;
using TestJuntoSeguros.Data.Context;

namespace TestJuntoSeguros.Api.Configurations
{
  public static class IdentityConfig
  {
    public static void AddCustomIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
      Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

      services.AddDbContext<IdentityContext>(options =>
          options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
          b => b.MigrationsAssembly("TestJuntoSeguros.Data")));

      services.AddCustomIdentity<IdentityUser>(options =>
      {
        options.SignIn.RequireConfirmedEmail = false;
        options.Lockout.MaxFailedAccessAttempts = 5;
      }).AddCustomRoles<IdentityRole>()
          .AddCustomEntityFrameworkStores<IdentityContext>()
          .AddDefaultTokenProviders();

      services.AddJwtConfiguration(configuration, "AppSettings");
    }
  }
}
