using System;
using Microsoft.Extensions.DependencyInjection;
using TestJuntoSeguros.IoC;

namespace TestJuntoSeguros.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            NativeInjector.RegisterServices(services);
        }
    }
}